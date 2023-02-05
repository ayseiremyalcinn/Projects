import org.knowm.xchart.*;
import org.knowm.xchart.style.Styler;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.io.*;


class Main {
    public static void main(String args[]) throws IOException {

        BufferedReader br = new BufferedReader(new FileReader(args[0]));
        ArrayList<Integer> file = new ArrayList<>();
        boolean first = true;
        while (true) {
            String data = br.readLine();
            if (data != null) {
                if(first){
                    first = false;
                }else{
                    file.add(Integer.parseInt(data.split(",")[7]));
                }

            } else {
                break;
            }
        }


        double[] Complexity_InsertionR = new double[10];
        double[] Complexity_MergeR = new double[10];
        double[] Complexity_PigeonholeR = new double[10];
        double[] Complexity_CountingR = new double[10];

        double[] Complexity_InsertionS = new double[10];
        double[] Complexity_MergeS = new double[10];
        double[] Complexity_PigeonholeS = new double[10];
        double[] Complexity_CountingS = new double[10];

        double[] Complexity_InsertionRS = new double[10];
        double[] Complexity_MergeRS = new double[10];
        double[] Complexity_PigeonholeRS = new double[10];
        double[] Complexity_CountingRS = new double[10];


        SortingAlgorithms sortingAlgorithms = new SortingAlgorithms();

        int size = 512;
        for (int i = 0; i < 10; i++) {
            if(i == 9){ size = file.size()-1;}

            int max = file.get(0), min =file.get(0);
            int[] arr = new int[size];

            for (int j = 0; j < size; j++) {
                arr[j] = file.get(j);

                if(file.get(j) > max)
                    max = file.get(j);
                if(file.get(j) < min)
                    min = file.get(j);
            }
            Complexity_InsertionR[i] = sortingAlgorithms.InsertionSort(arr);
            Complexity_MergeR[i] = sortingAlgorithms.MergeSort(arr);
            Complexity_PigeonholeR[i] = sortingAlgorithms.PigeonholeSort(arr, max, min);
            Complexity_CountingR[i] = sortingAlgorithms.CountingSort(arr, max);

            arr = sortingAlgorithms.sortList(arr);

            Complexity_InsertionS[i] = sortingAlgorithms.InsertionSort(arr);
            Complexity_MergeS[i] = sortingAlgorithms.MergeSort(arr);
            Complexity_PigeonholeS[i] = sortingAlgorithms.PigeonholeSort(arr, max, min);
            Complexity_CountingS[i] = sortingAlgorithms.CountingSort(arr, max);

            int[] reversed = new int[size];
            for (int j = 0; j < size; j++) {
                reversed[size-j-1] = arr[j];
            }

            Complexity_InsertionRS[i] = sortingAlgorithms.InsertionSort(reversed);
            Complexity_MergeRS[i] = sortingAlgorithms.MergeSort(reversed);
            Complexity_PigeonholeRS[i] = sortingAlgorithms.PigeonholeSort(reversed, max, min);
            Complexity_CountingRS[i] = sortingAlgorithms.CountingSort(reversed, max);

            size = size * 2;
        }



        // X axis data
        int[] inputAxis = {512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 251282};

        // Create sample data for linear runtime
        double[][] yRandom = new double[4][10];
        yRandom[0] = Complexity_InsertionR;
        yRandom[1] = Complexity_MergeR;
        yRandom[2] = Complexity_PigeonholeR;
        yRandom[3] = Complexity_CountingR;

        double[][] ySorted = new double[4][10];
        ySorted[0] = Complexity_InsertionS;
        ySorted[1] = Complexity_MergeS;
        ySorted[2] = Complexity_PigeonholeS;
        ySorted[3] = Complexity_CountingS;

        double[][] yReversed = new double[4][10];
        yReversed[0] = Complexity_InsertionRS;
        yReversed[1] = Complexity_MergeRS;
        yReversed[2] = Complexity_PigeonholeRS;
        yReversed[3] = Complexity_CountingRS;



        // Save the char as .png and show it
        showAndSaveChart("Test on Random Data", inputAxis, yRandom);
        showAndSaveChart("Test on Sorted Data", inputAxis, ySorted);
        showAndSaveChart("Test on Reverse Sorted Data", inputAxis, yReversed);

    }

    public static void showAndSaveChart(String title, int[] xAxis, double[][] yAxis) throws IOException {
        // Create Chart
        XYChart chart = new XYChartBuilder().width(800).height(600).title(title)
                .yAxisTitle("Time in Milliseconds").xAxisTitle("Input Size").build();

        // Convert x axis to double[]
        double[] doubleX = Arrays.stream(xAxis).asDoubleStream().toArray();

        // Customize Chart
        chart.getStyler().setLegendPosition(Styler.LegendPosition.InsideNE);
        chart.getStyler().setDefaultSeriesRenderStyle(XYSeries.XYSeriesRenderStyle.Line);

        // Add a plot for a sorting algorithm
        chart.addSeries("Insertion Sort", doubleX, yAxis[0]);
        chart.addSeries("Merge Sort", doubleX, yAxis[1]);
        chart.addSeries("Pigeonhole Sort", doubleX, yAxis[2]);
        chart.addSeries("Counting Sort", doubleX, yAxis[3]);

        // Save the chart as PNG
        BitmapEncoder.saveBitmap(chart, title + ".png", BitmapEncoder.BitmapFormat.PNG);

        // Show the chart
        new SwingWrapper(chart).displayChart();
    }
}
