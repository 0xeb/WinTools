cmake_minimum_required(VERSION 3.12)
project(Wizmo VERSION 1.0)

# Define executable target
add_executable(${PROJECT_NAME} 
    main.cpp
    Wizmo.cpp Wizmo.h)

# Use the multithreaded, static version of the runtime library for Release builds
if(MSVC)
    target_compile_options(${PROJECT_NAME} PRIVATE 
        $<$<CONFIG:Release>:/MT> # for compiling
        $<$<NOT:$<CONFIG:Release>>:/MD> # default to /MD for other configurations
    )
endif()

# Set target properties to output different executable names based on architecture
if(CMAKE_GENERATOR_PLATFORM MATCHES "x64")
    set_target_properties(${PROJECT_NAME} PROPERTIES OUTPUT_NAME "${PROJECT_NAME}64")
else()
    set_target_properties(${PROJECT_NAME} PROPERTIES OUTPUT_NAME "${PROJECT_NAME}32")
endif()
