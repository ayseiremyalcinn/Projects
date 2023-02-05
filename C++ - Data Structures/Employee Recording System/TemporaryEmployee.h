#ifndef ASSIGNMENT2_TEMPORARYEMPLOYEE_H
#define ASSIGNMENT2_TEMPORARYEMPLOYEE_H
#include "Employee.h"

class TemporaryEmployee : public  Employee{
public:

    TemporaryEmployee(int id, int type, string name, string surname, string title, float salaryCoefficient,Date *birtday, Date *appointmentDay, int experience) : Employee(id, type, name, surname, title, salaryCoefficient, birtday, appointmentDay, experience) {
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

    friend bool operator < (TemporaryEmployee e1, TemporaryEmployee e2);
    friend bool operator == (TemporaryEmployee e1, TemporaryEmployee e2);
    friend bool operator > (TemporaryEmployee e1, TemporaryEmployee e2);
    friend bool operator >= (TemporaryEmployee e1, TemporaryEmployee e2);
};


#endif //ASSIGNMENT2_TEMPORARYEMPLOYEE_H
