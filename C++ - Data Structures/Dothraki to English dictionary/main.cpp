#include <iostream>
#include <fstream>
#include <sstream>

using namespace std;

struct Node{            // Node yapisi
    char info;
    int nextLen = 0;
    string value;
    Node* next[26];
};
Node* getNode();
Node* root = getNode();


int find(Node* parent, char letter);
void insert(string key, string value);
void list(Node* parent, string temp);
string search(string dothraki);
void Delete(string keyWord);


int main(int argc, char *argv[]) {

    //............................TANIMLAMALAR............................................

    ifstream input(argv[1]);        // input dosyasini okumak icin stream
    freopen(argv[2],"w",stdout);

    string line;

    //.....................................................................................


    //.....................................................................................
    while(getline(input, line, '\n')){
       if(line.substr(0,4) == "list"){
            list(root, "");
       }else{
           bool flag = false;
           bool flag1 = false;
           string parametre;
           string parametre1;
           for(int i = 0; i < line.length(); i++){
               if(line.at(i) == ')'){
                   flag = false;
                   flag1 = false;
               }
               if(flag1){
                   parametre1 += line.at(i);
               }
               if(line.at(i) == ','){
                   flag = false;
                   flag1 = true;
               }
               if(flag){
                   parametre += line.at(i);
               }
               if(line.at(i) == '('){
                   flag = true;
               }
           }
           if(line.substr(0,4) == "sear"){
               cout << search(parametre) << endl;
           }else if(line.substr(0,4) == "dele"){
               Delete(parametre);
           }else{
               insert(parametre, parametre1);
           }
       }
    }

    //......................................................................................
    return 0;
}



    //.................................FONKSIYONLAR...........................................

void insert(string key, string value){
    Node* p = root;
    for(int i = 0; i< key.length(); i++){
        int next = find(p, key.at(i));
        if(next == -1){
            for(int j = 0; j <26; j++){
                if(p->next[j] == nullptr){
                    p->next[j] = getNode();
                    p->next[j]->info = key.at(i);
                    p->nextLen++;
                    p = p->next[j];
                    break;
                }
            }
        }else{
            p = p->next[next];
        }
    }
    if(p->value.empty()){
        p->value = value;
        cout  << key << " was added" << endl;
    }
    else if(p->value == value){
        cout  <<key << " already exist" << endl;
    }else{
        p->value = value;
        cout  <<key << " was updated" << endl;
    }


}
string search(string dothraki){
    Node* p = root;
    for(int i = 0; i < dothraki.length(); i++){

        int index = find(p, dothraki.at(i));
        if(index == -1 && i == 0){
            return "no record";
        }
        else if(index == -1){
            return "incorrect Dothraki word";
        }
        else{
            p = p->next[index];
        }

    }
    if(!p->value.empty()){
        return "The English equivalent is " + p->value;
    }else{
        return "not enough Dothraki word";
    }
}

void Delete(string keyWord){
    string text = search(keyWord);
    if(text.substr(0, 3) != "The"){
        cout << text << endl ;
    }
    else{
        Node* p = root;
        struct Point{
            Node* node;
            int index;
        };
        Point brakingPoint;
        brakingPoint.node = p;
        brakingPoint.index = 0;
        for(int i = 0; i < keyWord.length(); i++){
            int index = find(p, keyWord.at(i));
            p = p->next[index];
            if(p->nextLen != 1){

                if(i == keyWord.length()-1){
                    p->value = "";
                    cout <<keyWord << " deletion is successful" << endl;
                    if(p->nextLen != 0){
                        return;
                    }
                    break;
                }
                index = find(p, keyWord.at(i+1));
                brakingPoint.node = p;
                brakingPoint.index = index;
            }
        }
        brakingPoint.node->next[brakingPoint.index] = nullptr;
        brakingPoint.node->nextLen--;
    }
}


int find(Node* parent, char letter){
    if(parent->nextLen != 0){
        for(int i = 0; i < 26; i++){
            if(parent->next[i] != nullptr && parent->next[i]->info == letter){
                return i;
            }
        }
    }
    return -1;
}

Node* getNode(){
    Node* pNode = new Node;

    for (int i = 0; i < 26; i++)
        pNode->next[i] = nullptr;
    return pNode;
}

void list(Node* parent, string temp){
    if(parent->nextLen != 0){
        for(int i = 0; i < 26; i++){
            if(parent->next[i] != nullptr){
                if(parent->nextLen != 1 && parent != root){
                    cout << "\n\t-" << temp;

                    cout << parent->next[i]->info;
                    if(!parent->next[i]->value.empty()){
                        cout << "(" << parent->next[i]->value << ")" << endl;
                    }
                    list(parent->next[i], temp + parent->next[i]->info);

                }
                else{
                    if(parent->nextLen != 1){
                        cout << "-";
                    }
                    cout << parent->next[i]->info;
                    if(!parent->next[i]->value.empty()){
                        cout << "(" << parent->next[i]->value << ")" << endl;
                    }
                    list(parent->next[i], temp + parent->next[i]->info);
                }
            }
        }
    }
}
