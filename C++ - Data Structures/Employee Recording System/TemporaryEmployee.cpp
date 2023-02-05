#include "TemporaryEmployee.h"

bool operator<(TemporaryEmployee e1, TemporaryEmployee e2) {
    return e1.getId() < e2.getId();
}

bool operator==(TemporaryEmployee e1, TemporaryEmployee e2) {
    return e1.getId() == e2.getId();
}

bool operator>(TemporaryEmployee e1, TemporaryEmployee e2) {
    return e1.getId() > e2.getId();
}

bool operator>=(TemporaryEmployee e1, TemporaryEmployee e2) {
    return e1.getId() > e2.getId() || e1.getId() == e2.getId();
}
