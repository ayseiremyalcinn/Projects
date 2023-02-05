#include <iostream>
#include <fstream>
#include <sstream>
using namespace std;

int map[4][2] = {{0,-1},{-1,0},{0,1}, {1,0}};
int N, n, score;
int **grid, **grid2;

int main(int argc, char *argv[]) {

    // .........................TANIMLAMALAR...........................

    ifstream input1(argv[1]), input2(argv[2]);  // input dosyalarini okumak icin stream
    ofstream output(argv[3]);
    string satir, line, number, balon, location;
    int count = 0;
    bool first = true;
    bool check(int t, int x, int y);


    //..............................PART1..............................

    while(getline(input1, satir, '\n')){
        int counter = 0;
        int command[] = {0, 0, 0};
        stringstream ss(satir);
        while(getline(ss, number, ' ')){      //  komutlar split edildi command arryinde.
            command[counter] = stoi(number);
            counter++;
        }
        if(first){                                        //  grid ilk input kullanilarak olusturuldu.
            N = command[0];
            grid = new int*[N];
            for(int i = 0; i < N; i++) {
                grid[i] = new int[N];
            }
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < N; j++) {
                    grid[i][j] = 0;
                }
            }
            first = false;
        }else{
            int t = command[0];
            grid[command[1]][command[2]] = t;
            while(check(t, command[1], command[2])){      // :)
                t = t + 1;
            }
        }
    }
    output << "PART 1:" << endl;
    for (int i = 0; i < N; i++) {
        for (int j = 0; j < N; j++) {
            output << grid[i][j] << " ";
        }
        output << endl;
    }
    input1.close();

    //...................................PART2........................................

    while(getline(input2, line, '\n')){

        if(count == 0){                                     // ilk input satiri
            n = stoi(line);
            grid2 = new int*[n];
            for(int i = 0; i < n; i++) {
                grid2[i] = new int[n];
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    grid2[i][j] = 0;
                }
            }

        }
        else if(0 < count && count < n+1){                     // matrix
            stringstream ss(line);
            int count2 = 0;
            while(getline(ss, balon, ' ')){
                grid2[count-1][count2] = stoi(balon);
                count2++;
            }
        }else{                                               //............komutlar...............
            stringstream ss(line);
            int x=0, y=0;
            bool bool0 = true;
            while(getline(ss, location, ' ')){      // x ve y yi bulmak icin
                if(bool0){
                    x = stoi(location);
                    bool0 = false;
                }
                else  { y = stoi(location);}
            }
            int bomb = grid2[x][y];
            for (int i = 0; i < n; i++) {                                     // tum gridi dolasir
                for (int j = 0; j < n; j++) {
                    if(abs(x-i) == abs(y-j) || i == x || j == y){    // ayni satir sutun veya caprazda bulundugu balonlari ayiklar
                        if(grid2[i][j] == bomb){                           // bomba varsa
                            grid2[i][j] = 0;                              // patlatir
                            score += bomb;                               // puani hesaplar
                        }
                    }
                }
            }
        }
        count++;
    }
    output << endl << "PART 2:" << endl;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            output << grid2[i][j] << " ";
        }
        output << endl;
    }
    output << "Final Point: " << score << "p";

    input2.close();
    output.close();

    return 0;
}
//....................................FONKSIYONLAR....................................

void remove(int x, int y){
    grid[x][y] = 0;
}
bool checkMinor(int t, int x, int y, int lastX, int lastY){

    for (int i = 0; i < 4; i++) {
        int nextX = map[i][0] + x, nextY = map[i][1] + y;
        if(nextX >= 0 && nextY >= 0 && nextX != N && nextY != N &&  !(nextX == lastX && nextY == lastY)) {  // duvarlar ve onceki balon icin onlem alindi.
            if (grid[nextX][nextY] == t) {  // doluysa
                remove(x,y);
                remove(nextX, nextY);
                checkMinor(t, nextX, nextY, x, y);
                return true;
            }
        }
    }
    return false;
}
bool check(int t, int x, int y){
    int neighbour = 0;
    bool start = false;
    for (int i = 0; i < 4; i++) {
        int nextX = map[i][0] + x, nextY = map[i][1] + y;
        if(nextX >= 0 && nextY >= 0 && nextX != N && nextY != N) {  // duvarlar icin onlem alindi.
            if (grid[nextX][nextY] == t) {  // doluysa
                if (checkMinor(t, nextX, nextY, x, y) || start) {
                    grid[x][y] = t + 1;
                    start = true;
                }else{
                    neighbour++;
                }
            }
        }
    }
    if(neighbour > 1 ){
        start = true;
        grid[x][y] = t + 1;
    }

    if(start){
        for (int i = 0; i < 4; i++) {
            int nextX = map[i][0] + x, nextY = map[i][1] + y;
            if(nextX >= 0 && nextY >= 0 && nextX != N && nextY != N){
                if (grid[nextX][nextY] == t){
                    remove(nextX, nextY);
                }
            }
        }
        return true;
    }else{
        return false;
    }
}