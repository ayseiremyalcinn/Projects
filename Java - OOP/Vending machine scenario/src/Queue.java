import java.util.Arrays;

public class Queue {
    int capacity = 0;
    int index;
    public Tokens[] queue = new Tokens[capacity];

    public void enQueue(Tokens value){
        capacity++;
        Tokens[] newQueue = new Tokens[capacity];

        System.arraycopy(queue, 0, newQueue, 1, index);
        newQueue[0] = value;
        queue = newQueue;
        index++;
        System.out.println(Arrays.toString(queue));
        sorting();
    }

    public void sorting(){
        Tokens[] list = new Tokens[this.size()];
        int sayac = 0;
        for (int i = 0; i < this.size(); i++) {
            if(queue[i] != null){
                list[i] = queue[i];
            }else{
                sayac++;
            }
        }
        Arrays.sort(list);
        queue = list;
        for (int i = 0; i < sayac; i++) {
            enQueue(null);
        }
    }
// i loves i

    public void deQueue(){
        Tokens[] newQueue = new Tokens[capacity];
        System.arraycopy(queue, 1, newQueue, 0, size());
        queue = newQueue;
        index--;
        if(capacity/4 == index){
            shrink();
        }
    }

    public void remove(Tokens token){
        sorting();
        Queue newQueue = new Queue();
        for (Tokens t: queue){
            if(t != token){
                newQueue.enQueue(t);
            }
        }
        capacity--;
        index--;
        this.queue = newQueue.queue;

    }

    public void shrink(){
        capacity = capacity/2;
        Tokens[] newQueue = new Tokens[capacity];
        System.arraycopy(queue, 0, newQueue, 0, size());
        queue = newQueue;
    }

    public int size(){
        return index;
    }

    public Tokens findMaxToken(String type){
        for (int i = queue.length-1; i > 0; i--) {
            if(queue[i].type.equals(type)){
                return queue[i];
            }
        }
        return null;
    }
}
