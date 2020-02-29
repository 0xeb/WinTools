cmake_minimum_required(VERSION 3.2)
project(Wizmo VERSION 1.0)

set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT")

add_executable(Wizmo 
    main.cpp
    Wizmo.cpp Wizmo.h)

