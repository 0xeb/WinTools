cmake_minimum_required(VERSION 3.12 FATAL_ERROR)
project(WorkingSet)

set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT")

add_executable(VerifyDll VerifyDll.cpp)

add_library(DummyDll SHARED dummydll.cpp)