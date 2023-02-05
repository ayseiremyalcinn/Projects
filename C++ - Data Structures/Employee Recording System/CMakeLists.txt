cmake_minimum_required(VERSION 3.19)
project(assignment2)

set(CMAKE_CXX_STANDARD 11)

add_executable(assignment2 main.cpp Employee.h TemporaryEmployee.cpp TemporaryEmployee.h PermanentEmployee.cpp PermanentEmployee.h CircularArrayLinkedList.h DoubleDynamicLinkedList.h Date.h)