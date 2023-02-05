import java.io.*;
import java.nio.charset.StandardCharsets;

public class Stack {


    int capacity = 2;
    String[] stack = new String[capacity];
    int index = 0;

    public void push(String value){
        if(index+1 == capacity){
            expand();
        }
        stack[index] = value;
        index++;
    }

    public void expand(){
        capacity = capacity * 2;
        String[] newStack = new String[capacity];
        System.arraycopy(stack, 0, newStack, 0, index+1);
        stack = newStack;
    }

    public void pop(){
        index--;
        stack[index] = null;
        if(capacity/4 == index){
            shrink();
        }
    }

    public void shrink(){
        capacity = capacity/2;
        String[] newStack = new String[capacity];
        System.arraycopy(stack, 0, newStack, 0, index+1);
        stack = newStack;
    }

    public int size(){
        return index;
    }

    public void print(String output) throws IOException {
        BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(output, true), StandardCharsets.UTF_8));

        for(String i: stack){
            writer.write(i + "\n" );
        }
        writer.close();

    }

}
