
#include <iostream>
#include "PermanentEmployee.h"

using namespace std;
struct node
{
    Employee* info;
    node* next;
    node* prev;

};
class DoubleDynamicLinkedList {
public:

    node *head = new node();

    DoubleDynamicLinkedList() {
        head->next = nullptr;
        head->prev = nullptr;
    }
    node* getNode(Employee* employee){
        node *NewNode = new node();
        NewNode->next = nullptr;
        NewNode->prev = nullptr;
        NewNode->info = employee;
    }


    bool IsEmpty(){
        return head->info == nullptr;

    }
    bool IsContain(Employee* employee){
        for(node* i = head; i != nullptr; i = i->next){
            if(i->info == employee){
                return true;
            }
        }
        return false;
    }
    Employee* findId(int id){
        for(node* i = head; i != nullptr; i = i->next){
            if(i->info->getId() == id){
                return i->info;
            }
        }
        return nullptr;
    }

    void print(){
        for(node* i = head; i != nullptr; i = i->next){
            cout << "Appointment date:  " << i->info->getAppointmentDay()->getDay() << "-"<< i->info->getAppointmentDay()->getMonth()<< "-"<< i->info->getAppointmentDay()->getYear() << "    Employee name: "<<i->info->getName()<< " "<< i->info->getSurname() << endl;
        }
    }



    void push(Employee* employee){
        if(IsEmpty()){
            head->info = employee;
        }else{
            node* last = getNode(nullptr);
            for(node* i = head; i != nullptr; i = i->next){

                if(IsSmall(employee , i->info)){
                    node* newNode = getNode(employee);
                    newNode->next = i;
                    newNode->prev = i->prev;

                    if(i == head){
                        i->prev = newNode;
                        head = newNode;
                        return;
                    }else{
                        i->prev->next = newNode;
                        i->prev = newNode;
                        return;
                    }
                }
                last = i;
            }
            node* newNode = getNode(employee);
            newNode->next = nullptr;
            newNode->prev = last;
            last->next = newNode;

        }
    }

    void remove(Employee* employee){
        for(node* i = head; i != nullptr; i = i->next){
            if(i->info == employee){
                if(i == head){
                    head = i->next;
                    i->next->prev = nullptr;
                }else{
                    i->prev->next = i->next;
                    if(i->next != nullptr) {
                        i->next->prev = i->prev;
                    }
                }
            }
        }
    }
    bool IsSmall(Employee* e1, Employee* e2){

        if(e1->getAppointmentDay()->getYear() != e2->getAppointmentDay()->getYear()){
            return e1->getAppointmentDay()->getYear() < e2->getAppointmentDay()->getYear();
        }
        else if(e1->getAppointmentDay()->getMonth() != e2->getAppointmentDay()->getMonth()){
            return e1->getAppointmentDay()->getMonth() < e2->getAppointmentDay()->getMonth();
        }
        else if(e1->getAppointmentDay()->getDay() != e2->getAppointmentDay()->getDay()){
            return e1->getAppointmentDay()->getDay() < e2->getAppointmentDay()->getDay();
        }
        else{
            return false;
        }
    }
};


