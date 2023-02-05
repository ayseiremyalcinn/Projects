

#include "PermanentEmployee.h"

bool operator<(PermanentEmployee e1, PermanentEmployee e2) {
    if(e1.getAppointmentDay()->getYear() != e2.getAppointmentDay()->getYear()){
        return e1.getAppointmentDay()->getYear() < e2.getAppointmentDay()->getYear();
    }
    else if(e1.getAppointmentDay()->getMonth() != e2.getAppointmentDay()->getMonth()){
        return e1.getAppointmentDay()->getMonth() < e2.getAppointmentDay()->getMonth();
    }
    else if(e1.getAppointmentDay()->getDay() != e2.getAppointmentDay()->getDay()){
        return e1.getAppointmentDay()->getDay() < e2.getAppointmentDay()->getDay();
    }
    else{
        return false;
    }
}

bool operator==(PermanentEmployee e1, PermanentEmployee e2) {
    return e1.getAppointmentDay()->getYear() == e2.getAppointmentDay()->getYear() && e1.getAppointmentDay()->getMonth() != e2.getAppointmentDay()->getMonth() && e1.getAppointmentDay()->getDay() != e2.getAppointmentDay()->getDay();

}

bool operator>(PermanentEmployee e1, PermanentEmployee e2) {
    return !(e1 < e2) && !(e1 == e2);
}
