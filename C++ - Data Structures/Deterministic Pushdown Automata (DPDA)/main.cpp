#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <string>
#include <stack>

using namespace std;
string arrayT[100][5];
string starter;
stack<string> myStack;

int main(int argc, char *argv[]) {


    // .........................TANIMLAMALAR...........................

    ifstream Dfile(argv[1]);
    ifstream input(argv[2]);  // input dosyalarini okumak icin stream
    freopen(argv[3],"w",stdout);
    string line;

    string lasts[10];
    string arrayA[50];
    string arrayZ[50];
    string arrayQ[50];
    void DPDA(int index, string read);
    string strip(string &inpt);
    int findNext(string state, string read, string pop);

    //..................................................................


    //............................SETUP.................................

    string inputs[50][2];
    int counter = 0;
    while(getline(Dfile, line, '\n')){
        string letter;
        string letters[2];
        stringstream ss(line);
        while(getline(ss, letter, ':')){
            if(letter == "Q" ||letter == "A" ||letter == "Z" ||letter == "T"){
                letters[0] = letter;
            }
            else{
                letters[1] = letter;
            }
        }
        inputs[counter][0] = letters[0];
        inputs[counter][1] = letters[1];
        counter++;


    }
    int bigcounter = 0;
    for(int i = 0; i< 50; i++){
        if(inputs[i][0]== "A"){
            stringstream ss(inputs[i][1]);
            string character;
            int count = 0;
            while(getline(ss, character, ',')){
                arrayA[count] = character;
                count ++;
            }
        }
        else if(inputs[i][0]== "Z"){
            stringstream ss(inputs[i][1]);
            string character;
            int count = 0;
            while(getline(ss, character, ',')){
                arrayZ[count] = character;
                count ++;
            }
        }
        else if(inputs[i][0]== "Q"){
            stringstream ss(inputs[i][1]);
            string part;
            int count = 0;
            while(getline(ss, part, ' ')){
                if(count == 0|| count == 2){
                    stringstream sss(part);
                    string character;
                    int co = 0;
                    while(getline(sss, character, ',')){
                        if(count == 0){
                            arrayQ[co] = character;
                        }else{
                            if(co == 0){
                                starter = strip(character);
                            }else{
                                lasts[co-1] = strip(character);
                            }
                        }

                        co++;
                    }
                }
                count ++;
            }
        }
        else if(inputs[i][0]== "T"){
            stringstream ss(inputs[i][1]);
            string character;
            int count = 0;
            while(getline(ss, character, ',')){
                arrayT[bigcounter][count] = character;
                count ++;
            }
            bigcounter++;
        }
        else{
            break;
        }
    }

    //.....................................................................

    //.......................The Game Is On................................

    while(getline(input, line, '\n')){
        if(!line.empty()){
            stringstream ss(line);
            string letter;
            while(!myStack.empty()){
                myStack.pop();
            }
            int index;
            for(int i = 0; i< 100; i++){
                if(arrayT[i][0] == starter){
                    index = i;
                    break;
                }
            }
            bool flag = true;

            while(getline(ss, letter, ',')){

                if(flag){
                    DPDA(index, letter);
                    flag = false;

                }else{
                    index = findNext(arrayT[index][3], letter, myStack.top());
                    DPDA(index, letter);
                }
                string read0 = arrayT[index][1];

                while(read0 == "e"){
                    cout << arrayT[index][0] << "," << arrayT[index][1] << "," << arrayT[index][2] << " => " <<  arrayT[index][3] << "," << arrayT[index][4] << " [STACK]:" ;
                    stack<string> printStack = myStack;
                    stack<string> temp;
                    while (!printStack.empty()){
                        temp.push(printStack.top());
                        printStack.pop();
                    }
                    while (!temp.empty()){
                        cout << temp.top() << ",";
                        temp.pop();
                    }
                    cout<< endl;

                    index = findNext(arrayT[index][3], letter, myStack.top());
                    DPDA(index, letter);
                    read0 = arrayT[index][1];


                }
                cout << arrayT[index][0] << "," << arrayT[index][1] << "," << arrayT[index][2] << " => " <<  arrayT[index][3] << "," << arrayT[index][4] << " [STACK]:" ;
                stack<string> printStack = myStack;
                stack<string> temp;
                while (!printStack.empty()){
                    temp.push(printStack.top());
                    printStack.pop();
                }
                while (!temp.empty()){
                    cout << temp.top() << ",";
                    temp.pop();
                }
                cout<< endl;

            }
            if(!myStack.empty()){
                while(findNext(arrayT[index][3], "e", myStack.top()) > 0){
                    index = findNext(arrayT[index][3], "e", myStack.top());
                    DPDA(index, "e");
                }
            }else{
                while(findNext(arrayT[index][3], "e", "e") > 0){
                    index = findNext(arrayT[index][3], "e", "e");
                    DPDA(index, "e");
                }
            }

            cout << arrayT[index][0] << "," << arrayT[index][1] << "," << arrayT[index][2] << " => " <<  arrayT[index][3] << "," << arrayT[index][4] << " [STACK]:" ;
            stack<string> printStack = myStack;
            stack<string> temp;
            while (!printStack.empty()){
                temp.push(printStack.top());
                printStack.pop();
            }
            while (!temp.empty()){
                cout << temp.top() << ",";
                temp.pop();
            }
            cout<< endl;


            bool accept = false;
            for(int j = 0; j < lasts->length(); j++){
                if(lasts[j] == arrayT[index][3] || myStack.empty()){
                    cout << "ACCEPT\n"<< endl;
                    accept = true;
                    break;
                }
            }
            if(!accept){
                cout << "REJECT\n"<< endl;
            }
        }else{
            cout << "ACCEPT\n"<< endl;
        }


    } cout << "";



    return 0;
}

string strip(string &inpt) {
    string result;
    for (int i = 1; i < inpt.length() - 1; i++) {
        result += inpt[i];
    }
    return result;

}
int findNext(string state, string read, string pop){
    for(int i = 0; i< 100; i++){
        if(state == arrayT[i][0] && (arrayT[i][1] == "e" || read == arrayT[i][1]) && (arrayT[i][2] == "e" || pop == arrayT[i][2])|| pop == "e"){
            return i;
        }
    }
    return -1;
}
void DPDA(int index, string read){
    if( !myStack.empty() && ( arrayT[index][2] ==myStack.top())){
        myStack.pop();
    }
    if(arrayT[index][4] != "e"){
        myStack.push(arrayT[index][4]);
    }

}

