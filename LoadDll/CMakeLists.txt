cmake_minimum_required(VERSION 3.12 FATAL_ERROR)
project(LoadDll)

set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT")

add_executable(LoadDll LoadDll.cpp)

# Determine the system architecture
if(CMAKE_SIZEOF_VOID_P EQUAL 4)
    set(ARCH_SUFFIX "32")
elseif(CMAKE_SIZEOF_VOID_P EQUAL 8)
    set(ARCH_SUFFIX "64")
else()
    message(FATAL_ERROR "Unsupported system architecture")
endif()

set_target_properties(${PROJECT_NAME} PROPERTIES OUTPUT_NAME "${PROJECT_NAME}${ARCH_SUFFIX}")
