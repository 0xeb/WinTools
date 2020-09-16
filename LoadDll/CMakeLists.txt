cmake_minimum_required(VERSION 3.12 FATAL_ERROR)
project(LoadDll)

set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT")

add_executable(LoadDll LoadDll.cpp)
