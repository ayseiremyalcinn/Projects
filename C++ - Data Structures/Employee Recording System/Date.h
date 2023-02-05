
#ifndef ASSIGNMENT2_DATE_H
#define ASSIGNMENT2_DATE_H


class Date {

private:
    int day;
    int month;
    int year;

public:
    Date(int day, int month, int year){
        setDay(day);
        setMonth(month);
        setYear(year);
    }

    void setDay(int dayy) {
        Date::day = dayy;
    }

    void setMonth(int monthh) {
        Date::month = monthh;
    }

    void setYear(int yearr) {
        Date::year = yearr;
    }

    int getDay(){
        return day;
    }
    int getMonth(){
        return month;
    }

    int getYear(){
        return year;

    }
};





#endif //ASSIGNMENT2_DATE_H
