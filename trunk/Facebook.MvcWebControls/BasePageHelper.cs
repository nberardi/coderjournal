using System;
using System.Collections.Generic;
using System.Text;
using Facebook.Components;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;

namespace Facebook.MvcWebControls
{
    internal class BasePageHelper
    {
        #region Constants

        private const string SESSION_SESSION_KEY = "SessionKey";
        private const string SESSION_USER_ID = "UserId";
        private const string REQUEST_IN_CANVAS = "fb_sig_in_canvas";
        private const string REQUEST_SESSION_KEY = "fb_sig_session_key";
        private const string REQUEST_USER_ID = "fb_sig_user";
        private const string QUERY_AUTH_TOKEN = "auth_token";
        private const string FACEBOOK_LOGIN_URL = @"http://www.facebook.com/login.php?api_key=";
        private const string FACEBOOK_ADD_URL = @"http://www.facebook.com/add.php?api_key=";
        private const string FACEBOOK_CANVAS_PARAM = "&canvas";

        #endregion

        #region Private Members

        private FacebookService _fbService;

        private bool _useSession;

        private bool _autoAdd;

        private HttpRequest _request;

        private HttpResponse _response;

        private HttpSessionState _session;

        #endregion

        #region Constructors

        private BasePageHelper(FacebookService fbService, bool useSession, bool autoAdd, HttpRequest request,
            HttpResponse response, HttpSessionState session)
        {
            _fbService = fbService;
            _useSession = useSession;
            _autoAdd = autoAdd;
            _request = request;
            _response = response;
            _session = session;
        }

        #endregion

        #region Public Static Methods

        public static void LoadFBMLPage(FacebookService fbService, bool autoAdd, HttpRequest request, HttpResponse response)
        {
            string sessionKey = null;
            string userId = null;
            string inCanvas = request[REQUEST_IN_CANVAS];

            fbService.IsDesktopApplication = false;
                        
            if (string.IsNullOrEmpty(fbService.SessionKey) || string.IsNullOrEmpty(fbService.UserId))
            {
                sessionKey = request[REQUEST_SESSION_KEY];
                userId = request[REQUEST_USER_ID];
            }
            else
            {
                sessionKey = fbService.SessionKey;
                userId = fbService.UserId;
            }

            // When the user uses the facebook login page, the redirect back here will will have the auth_token in the query params
            string authToken = request.QueryString[QUERY_AUTH_TOKEN];

            // We have already established a session on behalf of this user
            if (!String.IsNullOrEmpty(sessionKey))
            {
                fbService.SessionKey = sessionKey;
                fbService.UserId = userId;
            }
            //// This will be executed when facebook login redirects to our page
            else if (!String.IsNullOrEmpty(authToken))
            {
                fbService.CreateSession(authToken);
            }
            else
            {
                //response.Redirect(@"http://www.facebook.com/login.php?api_key=" + fbService.ApplicationKey + @"&v=1.0");
                response.Write("<fb:redirect url=\"" + FACEBOOK_LOGIN_URL + fbService.ApplicationKey + @"&v=1.0" + "\"/>");
                response.End();
            }

            if (!fbService.IsAppAdded() && autoAdd)
            {
                if ((!string.IsNullOrEmpty(inCanvas) && inCanvas.Equals("1")) || (!string.IsNullOrEmpty(request.Url.ToString()) && request.Url.ToString().ToLower().Contains("facebook.com")))
                {
                    response.Write("<fb:redirect url=\"" + FACEBOOK_ADD_URL + fbService.ApplicationKey + @"&v=1.0" + "\"/>");
                    response.End();
                }
                else
                {
                    response.Redirect(FACEBOOK_ADD_URL + fbService.ApplicationKey + @"&v=1.0");
                }
            }
        }

        public static void LoadIFramePage(FacebookService fbService, bool useSession, bool autoAdd, HttpRequest request, HttpResponse response, HttpSessionState session)
        {
            new BasePageHelper(fbService, useSession, autoAdd, request, response, session).LoadIFramePage();
        }

        #endregion

        #region Private Methods

        private void LoadIFramePage()
        {
            _fbService.IsDesktopApplication = false;

            // When the user uses the facebook login page, the redirect back here
            // will will have the auth_token in the query params
            string authToken = _request.QueryString[QUERY_AUTH_TOKEN];
            string sessionKey = null;
            string userId = null;

            if (string.IsNullOrEmpty(_fbService.SessionKey) || string.IsNullOrEmpty(_fbService.UserId))
            {
                sessionKey = _request[REQUEST_SESSION_KEY];
                userId = _request[REQUEST_USER_ID];
            }
            else
            {
                sessionKey = _fbService.SessionKey;
                userId = _fbService.UserId;
            }

            // We have already established a session on behalf of this user
            EstablishSession(sessionKey, userId, authToken, true);
        }

        private void EstablishSession(string sessionKey, string userId, string authToken, bool retry)
        {
            try
            {
                object sessionKeyFromSession = _useSession ? _session[SESSION_SESSION_KEY] :
                    _request.Cookies[SESSION_SESSION_KEY];

                if (String.IsNullOrEmpty(sessionKey) && sessionKeyFromSession != null)
                {
                    sessionKey = _useSession ? sessionKeyFromSession.ToString() : ((HttpCookie)sessionKeyFromSession).Value;
                    userId = _useSession ? _session[SESSION_USER_ID].ToString() : _request.Cookies[SESSION_USER_ID].Value;
                }

                if (!String.IsNullOrEmpty(sessionKey))
                {
                    _fbService.SessionKey = sessionKey;
                    _fbService.UserId = userId;
                    SetSessionInfo(sessionKey, userId);
                }
                //// This will be executed when facebook login redirects to our page
                else if (!String.IsNullOrEmpty(authToken))
                {
                    _fbService.CreateSession(authToken);
                    SetSessionInfo(_fbService.SessionKey, _fbService.UserId);
                }
                else
                {
                    RedirectToLogin();
                    return;
                }

                TryAddApp();
            }
            catch (Facebook.Exceptions.FacebookTimeoutException)
            {
                if (retry)
                {
                    ClearSessionInfo();
                    EstablishSession(null, null, authToken, false);
                }
                else
                {
                    throw;
                }
            }
        }

        private void RedirectTopFrame(HttpResponse response, string url)
        {
            response.Write("<script type=\"text/javascript\">\n" +
                "if (parent != self) \n" +
                "top.location.href = \"" + url + @"&v=1.0" + "\";\n" +
                "else self.location.href = \"" + url + @"&v=1.0" + "\";\n" +
                "</script>");
            response.End();
        }

        private void SetSessionInfo(string sessionKey, string userId)
        {
            if (_useSession)
            {
                _session[SESSION_SESSION_KEY] = sessionKey;
                _session[SESSION_USER_ID] = userId;
            }
            else
            {
                _response.Cookies.Clear();
                _response.Cookies.Add(new HttpCookie(SESSION_SESSION_KEY, sessionKey));
                _response.Cookies.Add(new HttpCookie(SESSION_USER_ID, userId));
            }
        }

        private void ClearSessionInfo()
        {
            if (_useSession)
            {
                _session[SESSION_SESSION_KEY] = null;
                _session[SESSION_USER_ID] = null;
            }
            else
            {
                _response.Cookies.Clear();
            }
        }

        private void TryAddApp()
        {
            if (!_fbService.IsAppAdded() && _autoAdd)
            {
                if (_fbService.SessionKey != null)
                {
                    _response.Cookies.Clear();
                    RedirectTopFrame(_response, FACEBOOK_ADD_URL + _fbService.ApplicationKey);
                }
                else
                {
                    RedirectToLogin();
                }
            }
        }

        private void RedirectToLogin()
        {
            RedirectTopFrame(_response, FACEBOOK_LOGIN_URL + _fbService.ApplicationKey + FACEBOOK_CANVAS_PARAM);
        }

        #endregion
    }
}
