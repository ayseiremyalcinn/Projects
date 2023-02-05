#ifndef ASSIGNMENT2_EMPLOYEE_H
#define ASSIGNMENT2_EMPLOYEE_H
#include <iostream>
#include "Date.h"
using namespace std;

class Employee {
private:
    int id;
    int type;
    string name;
    string surname;
    string title;
    float salaryCoefficient;
    Date* birtday;
    Date* appointmentDay;
    int experience;
public:

    Employee(int id, int type,string name,string surname,string title, float salaryCoefficient, Date* birtday, Date* appointmentDay, int experience){
        setId(id);
        setType(type);
        setName(name);
        setSurname(surname);
        setTitle(title);
        setSalaryCoefficient(salaryCoefficient);
        setBirtday(birtday);
        setAppointmentDay(appointmentDay);
        setExperience(experience);
    }


    int getId() {
        return id;
    }

    int getType()  {
        return type;
    }

    string getName(){
        return name;
    }

    string getSurname(){
        return surname;
    }

    string getTitle() {
        return title;
    }

    float getSalaryCoefficient(){
        return salaryCoefficient;
    }

    Date* getBirtday(){
        return birtday;
    }

    Date* getAppointmentDay(){
        return appointmentDay;
    }

    int getExperience(){
        return experience;
    }

    void setId(int idd) {
        id = idd;
    }

    void setType(int typee) {
        type = typee;
    }

    void setName(string namee) {
        name = namee;
    }

    void setSurname(string surnamee) {
        surname = surnamee;
    }

    void setTitle(string titlee) {
        title = titlee;
    }

    void setSalaryCoefficient(float salaryCoefficientt) {
        salaryCoefficient = salaryCoefficientt;
    }

    void setBirtday(Date* birtdayy) {
        birtday = birtdayy;
    }

    void setAppointmentDay(Date* appointmentDayy) {
        appointmentDay = appointmentDayy;
    }

    void setExperience(int experiencee) {
        experience = experiencee;
    }
};


#endif //ASSIGNMENT2_EMPLOYEE_H
