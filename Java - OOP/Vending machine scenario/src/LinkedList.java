public class LinkedList<T> {
    Node<T> head = new Node<>();

    public void addAtBack(T value){

        if(head.data == null){
            head.data = value;
            head.next = null;
        }else{
            Node<T> node = head;
            while(node.next != null){
                node = node.next;
            }
            Node<T> n = new Node<>();
            node.next = n;
            n.data = value;
            n.next = null;
        }
    }


    public void addAtFront(T value){
        Node<T> node = new Node<>();
        node.data = value;
        if(head.data == null){
            node.next = null;
        }else{
            node.next = head;
        }
        head = node;
    }

    public void addAt(int index, T value){

        try {
            if(index == 0){
                addAtFront(value);
            }else{
                Node<T> nodeV = new Node<>();
                nodeV.data = value;
                Node<T> node = head;
                for (int i = 0; i < index-1; i++) {
                    node = node.next;
                }
                Node<T> tmp = node.next;
                node.next = nodeV;
                nodeV.next = tmp;
            }
        }
        catch (NullPointerException err){
            System.out.println("There is no such an index");
        }
    }


    public void removeAt(int index){

        try {
            if(index == 0){
                head = head.next;
            }
            else{
                Node<T> node = head;
                for (int i = 0; i < index-1; i++) {
                    node = node.next;
                }
                Node<T> nodeD = node.next;
                node.next = nodeD.next;
            }
        }
        catch (NullPointerException err){
            System.out.println("There is no such an index");
        }
    }


    public void removeFromFront(){
        removeAt(0);
    }

    public void removeFromBack(){
        Node<T> node = head;
        int index = 0;
        while(node.next != null){
            node = node.next;
            index++;
        }
        removeAt(index);
    }


    public void print(){
        if(head.data == null){
            System.out.println("There is no item");
        }else{
            Node<T> node = head;
            while(node.next != null){
                System.out.println(node.data);
                node = node.next;
            }
            System.out.println(node.data);
        }
    }
}
