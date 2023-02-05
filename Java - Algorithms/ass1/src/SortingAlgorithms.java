
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class SortingAlgorithms {



    public double InsertionSort(int[] inputFile){
        double startTime = System.nanoTime();

        sortList(inputFile);

        double endTime = System.nanoTime();
        return (endTime - startTime)/1000000;
    }
    public int[] sortList(int[] inputFile){
        for (int j = 0; j < inputFile.length; j++) {
            int key = inputFile[j];
            int i = j-1;
            while( i > 0 && inputFile[i] > key){
                inputFile[i+1] = inputFile[i];
                i = i-1;
            }
            inputFile[i+1] = key;
        }
        return inputFile;
    }

    public double MergeSort(int[] inputFile){
        double startTime = System.nanoTime();

        recursionMerge(inputFile);

        double endTime = System.nanoTime();
        return (endTime - startTime)/1000000;
    }

    public int[] recursionMerge(int[] A){
        int n = A.length;
        if(n<=1){
            return A;
        }
        int[] left = Arrays.copyOfRange(A,0, n/2);
        int[] right = Arrays.copyOfRange(A,n/2, n);
        left = recursionMerge(left);
        right = recursionMerge(right);
        return Merge(left, right);
    }
    public int[] Merge(int[] A, int[] B){
        int indexA = 0, indexB = 0,indexC = 0;
        int[] C = new int[A.length + B.length];
        while (indexA != A.length && indexB != B.length){
            if(A[0] > B[0]){
                C[indexC] = B[0];
                indexC++;
                indexB++;
            }
            else {
                C[indexC] = A[0];
                indexC++;
                indexA++;
            }
        }
        while(indexA != A.length){
            C[indexC] = A[0];
            indexC++;
            indexA++;
        }
        while(indexB != B.length){
            C[indexC] = B[0];
            indexC++;
            indexB++;
        }
        return C;
    }

    public double PigeonholeSort(int[] inputFile, int max, int min){
        double startTime = System.nanoTime();
        int range = max - min + 1;
        ArrayList<Integer> output = new ArrayList<>();
        List<Integer>[] holes = new List[range];
        for(int i=0;i<range;i++) {
            holes[i] = new ArrayList<>();
        }
        for (int i = 0; i < inputFile.length; i++) {
            holes[inputFile[i]-min].add(inputFile[i]);
        }
        for (int i = 0; i < range; i++) {
            output.addAll(holes[i]);
        }

        double endTime = System.nanoTime();
        return (endTime - startTime)/1000000;
    }

    public double CountingSort(int[] inputFile, int k){
        double startTime = System.nanoTime();
        int size = inputFile.length;
        int output[] = new int[size];
        int count[] = new int[k+1];


        for (int i = 0; i < size; i++)
            count[inputFile[i]]++;


        for (int i = 1; i <= k; i++)
            count[i] += count[i - 1];

        for (int i = size - 1; i >= 0; i--) {
            output[count[inputFile[i]] - 1] = inputFile[i];
            count[inputFile[i]]--;
        }

        double endTime = System.nanoTime();
        return (endTime - startTime)/1000000;
    }

}
