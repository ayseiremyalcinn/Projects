
#ifndef ASSIGNMENT2_PERMANENTEMPLOYEE_H
#define ASSIGNMENT2_PERMANENTEMPLOYEE_H
#include "Employee.h"

class PermanentEmployee : public Employee {
public:

    PermanentEmployee(int id, int type, string name, string surname, string title, float salaryCoefficient,Date *birtday, Date *appointmentDay, int experience) : Employee(id, type, name, surname, title, salaryCoefficient, birtday, appointmentDay, experience) {
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
    friend bool operator < (PermanentEmployee e1, PermanentEmployee e2);
    friend bool operator == (PermanentEmployee e1, PermanentEmployee e2);
    friend bool operator > (PermanentEmployee e1, PermanentEmployee e2);
};


#endif //ASSIGNMENT2_PERMANENTEMPLOYEE_H
