#include <iostream>
#include "TemporaryEmployee.h"

using namespace std;
struct Node
{
    Employee* info;
    int next;

};
class CircularArrayLinkedList {

public:
    struct Node List[40];

    int head = 0;
    void editList(){
        for(int i=0; i<40; i++){
            List[i].info = nullptr;
            List[i].next = -1;
        }
    }
    bool IsFull(){
        for(int i=0; i<40; i++){
            if(List[i].info == nullptr){
                return false;
            }
        }
        return true;
    }

    bool IsEmpty(){
        return (List[0].info == nullptr);
    }

    void push(Employee* employee){
        int index = 0;
        int before = head;

        if(!IsFull()){
            for(int i= head; i < 40; i++){
                if(List[i].info == nullptr){
                    List[i].info = employee;
                    index = i;
                    break;
                }
            }
            for(int i= head; i>=0 ; i = List[i].next){

                if(employee->getId() < List[i].info->getId() ){

                    if(i == head){
                        head = index;
                        List[index].next = before;
                        return;
                    }else{
                        List[index].next = List[before].next;
                        List[before].next = index;
                        return;
                    }

                }
                else if(List[i].next == -1 && i != head){
                    List[i].next = index;
                    return;
                }
                before = i;
            }
        }
    }

    int IsContain(Employee* employee){
        for(int i=head; i>= 0; i = List[i].next){
            if(employee == List[i].info){
                return i;
            }
        }
        return -1;
    }
    int findId(int id){
        for(int i=head; i>= 0; i = List[i].next){
            if(List[i].info->getId() == id){
                return i;
            }
        }
        return -1;
    }

    void remove(int index){

        for(int i=head; i>= 0; i = List[i].next){
            if(List[i].next == index){
                List[i].next = List[index].next;
                List[index].info = nullptr;
                List[index].next = -1;
                return;
            }
            else if(index == head){
                head = List[index].next;
                List[index].info = nullptr;
                List[index].next = -1;
                return;
            }
        }
    }

    void Print(){
        for(int i=head; i>=0; i = List[i].next){
            cout << "Employee number: " <<List[i].info->getId()  <<"    Employee name: "<<List[i].info->getName()<< " "<< List[i].info->getSurname() << endl;
        }
    }

};







