(*
 Autogenerated by Thrift

 DO NOT EDIT UNLESS YOU ARE SURE YOU KNOW WHAT YOU ARE DOING
*)

open Thrift
open Cassandra_types

class virtual iface :
object
  method virtual get : string option -> string option -> columnPath option -> ConsistencyLevel.t option -> columnOrSuperColumn
  method virtual get_slice : string option -> string option -> columnParent option -> slicePredicate option -> ConsistencyLevel.t option -> columnOrSuperColumn list
  method virtual multiget : string option -> string list option -> columnPath option -> ConsistencyLevel.t option -> (string,columnOrSuperColumn) Hashtbl.t
  method virtual multiget_slice : string option -> string list option -> columnParent option -> slicePredicate option -> ConsistencyLevel.t option -> (string,columnOrSuperColumn list) Hashtbl.t
  method virtual get_count : string option -> string option -> columnParent option -> ConsistencyLevel.t option -> int
  method virtual get_key_range : string option -> string option -> string option -> string option -> int option -> ConsistencyLevel.t option -> string list
  method virtual get_range_slice : string option -> columnParent option -> slicePredicate option -> string option -> string option -> int option -> ConsistencyLevel.t option -> keySlice list
  method virtual insert : string option -> string option -> columnPath option -> string option -> Int64.t option -> ConsistencyLevel.t option -> unit
  method virtual batch_insert : string option -> string option -> (string,columnOrSuperColumn list) Hashtbl.t option -> ConsistencyLevel.t option -> unit
  method virtual remove : string option -> string option -> columnPath option -> Int64.t option -> ConsistencyLevel.t option -> unit
  method virtual get_string_property : string option -> string
  method virtual get_string_list_property : string option -> string list
  method virtual describe_keyspace : string option -> (string,(string,string) Hashtbl.t) Hashtbl.t
end

class client : Protocol.t -> Protocol.t -> 
object
  method get : string -> string -> columnPath -> ConsistencyLevel.t -> columnOrSuperColumn
  method get_slice : string -> string -> columnParent -> slicePredicate -> ConsistencyLevel.t -> columnOrSuperColumn list
  method multiget : string -> string list -> columnPath -> ConsistencyLevel.t -> (string,columnOrSuperColumn) Hashtbl.t
  method multiget_slice : string -> string list -> columnParent -> slicePredicate -> ConsistencyLevel.t -> (string,columnOrSuperColumn list) Hashtbl.t
  method get_count : string -> string -> columnParent -> ConsistencyLevel.t -> int
  method get_key_range : string -> string -> string -> string -> int -> ConsistencyLevel.t -> string list
  method get_range_slice : string -> columnParent -> slicePredicate -> string -> string -> int -> ConsistencyLevel.t -> keySlice list
  method insert : string -> string -> columnPath -> string -> Int64.t -> ConsistencyLevel.t -> unit
  method batch_insert : string -> string -> (string,columnOrSuperColumn list) Hashtbl.t -> ConsistencyLevel.t -> unit
  method remove : string -> string -> columnPath -> Int64.t -> ConsistencyLevel.t -> unit
  method get_string_property : string -> string
  method get_string_list_property : string -> string list
  method describe_keyspace : string -> (string,(string,string) Hashtbl.t) Hashtbl.t
end

class processor : iface ->
object
  inherit Processor.t

  val processMap : (string, int * Protocol.t * Protocol.t -> unit) Hashtbl.t
  method process : Protocol.t -> Protocol.t -> bool
end
