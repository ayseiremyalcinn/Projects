#include <iostream>
#include <sstream>
#include "Employee.h"

#include "CircularArrayLinkedList.h"
#include "DoubleDynamicLinkedList.h"

int main() {

    DoubleDynamicLinkedList allEmployees;
    DoubleDynamicLinkedList doubleList;
    CircularArrayLinkedList circularList ;
    circularList.editList();

    while(true) {
        cout << "---- Employee Recording System ----\n"
                "Please select for the following Menu Operation:\n"
                "1) Appointment of a new employee\n"
                "2) Appointment of a transferred employee\n"
                "3) Updating the title and salary coefficient of an employee\n"
                "4) Deletion of an employee\n"
                "5) Listing the information of an employee\n"
                "6) Listing employees ordered by employee number\n"
                "7) Listing employees ordered by appointment date\n"
                "8) Listing employees appointed after a certain date\n"
                "9) Listing employees assigned in a given year\n"
                "10) Listing employees born before a certain date\n"
                "11) Listing employees born in a particular month\n"
                "12) Listing the information of the last assigned employee with a given title\n";

        int empId, empType, empExper, birtday, birtmonth, birtyear, appday, appmonth, appyear;
        string empName, empSur, empTitle;
        float empSalary;
        int command;
        cin >> command;

        if (command == 1 || command == 2) {

            cout << "Please Type the employee number" << endl;
            cin >> empId;
            cout << "Type the employee type" << endl;
            cin >> empType;
            cout << "Type the name" << endl;
            cin >> empName;
            cout << "Type the surname" << endl;
            cin >> empSur;
            cout << "Type the title" << endl;
            cin >> empTitle;
            cout << "Type the salary coefficient" << endl;
            cin >> empSalary;
            cout << "Type birth date" << endl;
            scanf("%d-%d-%d", &birtday, &birtmonth, &birtyear);
            Date *empBirtday = new Date(birtday, birtmonth, birtyear);

            cout << "Type appointment date" << endl;
            scanf("%d-%d-%d", &appday, &appmonth, &appyear);
            Date *empAppoint = new Date(appday, appmonth, appyear);
            if (command == 1) {
                cout << "Type lenght of service days" << endl;
                cin >> empExper;
            } else {
                empExper = 0;
            }
            if (empType == 0) {
                TemporaryEmployee *employee = new TemporaryEmployee(empId, empType, empName, empSur, empTitle,
                                                                    empSalary, empBirtday, empAppoint, empExper);
                if (circularList.IsContain(employee) == -1) {
                    circularList.push(employee);
                } else {
                    cout << "Employee is registered" << endl;
                }
            } else {
                PermanentEmployee *employee = new PermanentEmployee(empId, empType, empName, empSur, empTitle,
                                                                    empSalary, empBirtday, empAppoint, empExper);
                if (!doubleList.IsContain(employee)) {
                    doubleList.push(employee);
                } else {
                    cout << "Employee is registered" << endl;
                }
            }

        } else if (command == 3) {
            cout << "Please Type the employee number" << endl;
            cin >> empId;
            cout << "Please Type the new title" << endl;
            cin >> empTitle;
            cout << "Please Type the new salary coefficient" << endl;
            cin >> empSalary;

            int index = circularList.findId(empId);
            if (index != -1) {
                circularList.List[index].info->setTitle(empTitle);
                circularList.List[index].info->setSalaryCoefficient(empSalary);
            } else {
                PermanentEmployee *emp = (PermanentEmployee *) doubleList.findId(empId);
                if (emp != nullptr) {
                    emp->setTitle(empTitle);
                    emp->setSalaryCoefficient(empSalary);
                } else {
                    cout << "There is no such an employee" << endl;
                }
            }

        } else if (command == 4) {
            cout << "Please Type the employee number" << endl;
            cin >> empId;
            int index = circularList.findId(empId);
            PermanentEmployee *emp = (PermanentEmployee *) doubleList.findId(empId);
            if (index != -1) {
                circularList.remove(index);
            } else if (emp != nullptr) {
                doubleList.remove(emp);
            } else {
                cout << "There is no such an employee" << endl;
            }

        } else if (command == 5) {
            cout << "Please Type the employee number" << endl;
            cin >> empId;
            int index = circularList.findId(empId);
            PermanentEmployee *empi = (PermanentEmployee *) doubleList.findId(empId);
            if (index != -1) {
                TemporaryEmployee *emp = (TemporaryEmployee *) circularList.List[index].info;
                cout << "Employee ID: " << emp->getId() << endl;
                cout << "Employee name: " << emp->getName() << endl;
                cout << "Employee Surname: " << emp->getSurname() << endl;
                cout << "Employee Title: " << emp->getTitle() << endl;
                cout << "Employee Salary Coefficient: " << emp->getSalaryCoefficient() << endl;
                cout << "Employee Birt date: " << emp->getBirtday()->getDay() << "-" << emp->getBirtday()->getMonth()
                     << "-" << emp->getBirtday()->getYear() << endl;
                cout << "Employee Appointment: " << emp->getAppointmentDay()->getDay() << "-"
                     << emp->getAppointmentDay()->getMonth() << "-" << emp->getAppointmentDay()->getYear() << endl;
                cout << "Employee Length of service: " << emp->getExperience() << endl;
            } else if (empi != nullptr) {
                cout << "Employee ID: " << empi->getId() << endl;
                cout << "Employee name: " << empi->getName() << endl;
                cout << "Employee Surname: " << empi->getSurname() << endl;
                cout << "Employee Title: " << empi->getTitle() << endl;
                cout << "Employee Salary Coefficient: " << empi->getSalaryCoefficient() << endl;
                cout << "Employee Birt date: " << empi->getBirtday()->getDay() << "-" << empi->getBirtday()->getMonth()
                     << "-" << empi->getBirtday()->getYear() << endl;
                cout << "Employee Appointment: " << empi->getAppointmentDay()->getDay() << "-"
                     << empi->getAppointmentDay()->getMonth() << "-" << empi->getAppointmentDay()->getYear() << endl;
                cout << "Employee Length of service: " << empi->getExperience() << endl;
            } else {
                cout << "There is no such an employee" << endl;
            }

        } else if (command == 6) {
            CircularArrayLinkedList allEmployee;
            allEmployee.editList();
            for (int i = circularList.head; i >= 0; i = circularList.List[i].next) {
                allEmployee.push(circularList.List[i].info);
            }
            for (node *i = doubleList.head; i != nullptr; i = i->next) {
                allEmployee.push(i->info);
            }
            allEmployee.Print();
        } else if (command == 7 || command == 8 || command == 9 || command == 12) {

            for (int i = circularList.head; i >= 0; i = circularList.List[i].next) {
                allEmployees.push(circularList.List[i].info);
            }
            for (node *i = doubleList.head; i != nullptr; i = i->next) {
                allEmployees.push(i->info);
            }


            if (command == 7) {
                allEmployees.print();
            } else if (command == 8) {

                cout << "Type a date" << endl;
                scanf("%d-%d-%d", &appday, &appmonth, &appyear);
                Date *AppCert = new Date(appday, appmonth, appyear);
                PermanentEmployee *cert = new PermanentEmployee(0, 0, "", "", "", 0, AppCert, AppCert, 0);

                for (node *i = allEmployees.head; i != nullptr; i = i->next) {
                    if (allEmployees.IsSmall(cert, i->info)) {
                        cout << "Appointment date:  " << i->info->getAppointmentDay()->getDay() << "-"
                             << i->info->getAppointmentDay()->getMonth() << "-"
                             << i->info->getAppointmentDay()->getYear() << "    Employee name: " << i->info->getName()
                             << " " << i->info->getSurname() << endl;
                    }
                }
            } else if (command == 9) {
                cout << "Type a year" << endl;
                int year;
                cin >> year;
                for (node *i = allEmployees.head; i != nullptr; i = i->next) {
                    if (i->info->getAppointmentDay()->getYear() == year) {
                        cout << "Appointment date:  " << i->info->getAppointmentDay()->getDay() << "-"
                             << i->info->getAppointmentDay()->getMonth() << "-"
                             << i->info->getAppointmentDay()->getYear() << "    Employee name: " << i->info->getName()
                             << " " << i->info->getSurname() << endl;

                    }
                }
            } else {
                cout << "Type a title" << endl;
                cin >> empTitle;
                Employee *last;
                for (node *i = allEmployees.head; i != nullptr; i = i->next) {
                    if (empTitle == i->info->getTitle()) {
                        last = i->info;
                    }
                }
                cout << "Employee ID: " << last->getId() << "    Employee Name: " << last->getName() << " "
                     << last->getSurname() << "   Appointment Date: " << last->getAppointmentDay()->getDay() << "-"
                     << last->getAppointmentDay()->getMonth() << "-" << last->getAppointmentDay()->getYear() << endl;

            }

        } else if (command == 10 || command == 11) {
            CircularArrayLinkedList allEmployee;
            allEmployee.editList();
            for (int i = circularList.head; i >= 0; i = circularList.List[i].next) {
                allEmployee.push(circularList.List[i].info);
            }
            for (node *i = doubleList.head; i != nullptr; i = i->next) {
                allEmployee.push(i->info);
            }
            if (command == 10) {
                cout << "Type a date" << endl;
                scanf("%d-%d-%d", &appday, &appmonth, &appyear);

                for (int i = allEmployee.head; i >= 0; i = allEmployee.List[i].next) {
                    if (allEmployee.List[i].info->getBirtday()->getYear() < appyear) {
                        cout << "Employee ID: " << allEmployee.List[i].info->getId() << "    Employee Birt date: "
                             << allEmployee.List[i].info->getBirtday()->getDay() << "-"
                             << allEmployee.List[i].info->getBirtday()->getMonth() << "-"
                             << allEmployee.List[i].info->getBirtday()->getYear() << endl;
                    } else if (allEmployee.List[i].info->getBirtday()->getYear() == appyear) {
                        if (allEmployee.List[i].info->getBirtday()->getMonth() < appmonth) {
                            cout << "Employee ID: " << allEmployee.List[i].info->getId() << "    Employee Birt date: "
                                 << allEmployee.List[i].info->getBirtday()->getDay() << "-"
                                 << allEmployee.List[i].info->getBirtday()->getMonth() << "-"
                                 << allEmployee.List[i].info->getBirtday()->getYear() << endl;
                        } else if (allEmployee.List[i].info->getBirtday()->getMonth() == appmonth) {
                            if (allEmployee.List[i].info->getBirtday()->getDay() < appday) {
                                cout << "Employee ID: " << allEmployee.List[i].info->getId()
                                     << "    Employee Birt date: " << allEmployee.List[i].info->getBirtday()->getDay()
                                     << "-" << allEmployee.List[i].info->getBirtday()->getMonth() << "-"
                                     << allEmployee.List[i].info->getBirtday()->getYear() << endl;
                            }
                        }
                    }
                }
            } else {
                cout << "Type a month" << endl;
                int month;
                cin >> month;
                for (int i = allEmployee.head; i >= 0; i = allEmployee.List[i].next) {
                    if (allEmployee.List[i].info->getBirtday()->getMonth() == month) {
                        cout << "Employee ID: " << allEmployee.List[i].info->getId() << "    Employee Birt date: "
                             << allEmployee.List[i].info->getBirtday()->getDay() << "-"
                             << allEmployee.List[i].info->getBirtday()->getMonth() << "-"
                             << allEmployee.List[i].info->getBirtday()->getYear() << endl;
                    }
                }
            }
        }
    }
    return 0;
}
