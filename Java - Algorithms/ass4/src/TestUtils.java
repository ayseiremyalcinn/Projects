import java.io.*;
import java.util.List;

public class TestUtils {
    public static final String TEST_READ_NETWORK_OUTPUT_FILE = "test_io/output/test_read_network_output_1.dat";
    public static final String TEST_INITIALIZE_ROUTING_TABLES_OUTPUT_FILE = "test_io/output/test_initialize_routing_tables_output_1.dat";
    public static final String TEST_REMOVE_ROUTER_OUTPUT_FILE = "test_io/output/test_remove_router_output_1.dat";
    public static final String TEST_REMOVE_LINK_OUTPUT_FILE = "test_io/output/test_remove_link_output_1.dat";
    public static final String TEST_ADD_ROUTER_LINK_OUTPUT_FILE = "test_io/output/test_add_router_link_output_1.dat";
    public static final String TEST_CHANGE_LINK_COSTS_OUTPUT_FILE = "test_io/output/test_change_link_costs_output_1.dat";
    public static final String TEST_CHANGE_ROUTER_STATE_OUTPUT_FILE = "test_io/output/test_change_router_state_output_1.dat";

//    public static final String TEST_READ_NETWORK_OUTPUT_FILE = "test_io/output/test_read_network_output_2.dat";
//    public static final String TEST_INITIALIZE_ROUTING_TABLES_OUTPUT_FILE = "test_io/output/test_initialize_routing_tables_output_2.dat";
//    public static final String TEST_REMOVE_ROUTER_OUTPUT_FILE = "test_io/output/test_remove_router_output_2.dat";
//    public static final String TEST_REMOVE_LINK_OUTPUT_FILE = "test_io/output/test_remove_link_output_2.dat";
//    public static final String TEST_ADD_ROUTER_LINK_OUTPUT_FILE = "test_io/output/test_add_router_link_output_2.dat";
//    public static final String TEST_CHANGE_LINK_COSTS_OUTPUT_FILE = "test_io/output/test_change_link_costs_output_2.dat";
//    public static final String TEST_CHANGE_ROUTER_STATE_OUTPUT_FILE = "test_io/output/test_change_router_state_output_2.dat";

    public static final String ROUTER_INPUT_FILE = "test_io/input/router_regex_input.txt";
    public static final String LINK_INPUT_FILE = "test_io/input/link_regex_input.txt";
    public static final String ROUTER_EXPECTED_OUTPUT_FILE = "test_io/output/router_expected_regex_output.txt";
    public static final String LINK_EXPECTED_OUTPUT_FILE = "test_io/output/link_expected_regex_output.txt";

    public static double compareRoutingTables(List<RoutingTable> expectedTableList, List<RoutingTable> tableListUUT) {
        if(expectedTableList != null && tableListUUT != null && expectedTableList.size() > 0 && tableListUUT.size() > 0) {
            int totalEntries = 0;
            int correctEntries = 0;

            for(int i = 0; i < expectedTableList.size(); i++) {
                List<RoutingTableEntry> expectedTableEntryList = expectedTableList.get(i).getEntryList();
                List<RoutingTableEntry> tableEntryListUUT = tableListUUT.get(i).getEntryList();
                totalEntries += expectedTableEntryList.size();

                if (tableEntryListUUT.size() == expectedTableEntryList.size()) {
                    for (int j = 0; j < expectedTableEntryList.size(); j++) {
                        if(tableEntryListUUT.contains(expectedTableEntryList.get(j)))
                            correctEntries++;
                    }
                }
            }

            return (double) correctEntries / totalEntries;
        } else {
            return 0;
        }
    }

    public static Object readDatObject(String path) {
        try {
            FileInputStream fileIn = new FileInputStream(path);
            ObjectInputStream objectIn = new ObjectInputStream(fileIn);
            Object obj = objectIn.readObject();
            objectIn.close();
            return obj;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }
    }

    public static void enableOutput() {
        System.setOut(new PrintStream(new FileOutputStream(FileDescriptor.out)));
    }

    public static void disableOutput() {
        System.setOut(new java.io.PrintStream(new java.io.OutputStream() {
            @Override public void write(int b) {}
        }) {
            @Override public void flush() {}
            @Override public void close() {}
            @Override public void write(int b) {}
            @Override public void write(byte[] b) {}
            @Override public void write(byte[] buf, int off, int len) {}
            @Override public void print(boolean b) {}
            @Override public void print(char c) {}
            @Override public void print(int i) {}
            @Override public void print(long l) {}
            @Override public void print(float f) {}
            @Override public void print(double d) {}
            @Override public void print(char[] s) {}
            @Override public void print(String s) {}
            @Override public void print(Object obj) {}
            @Override public void println() {}
            @Override public void println(boolean x) {}
            @Override public void println(char x) {}
            @Override public void println(int x) {}
            @Override public void println(long x) {}
            @Override public void println(float x) {}
            @Override public void println(double x) {}
            @Override public void println(char[] x) {}
            @Override public void println(String x) {}
            @Override public void println(Object x) {}
            @Override public java.io.PrintStream printf(String format, Object... args) { return this; }
            @Override public java.io.PrintStream printf(java.util.Locale l, String format, Object... args) { return this; }
            @Override public java.io.PrintStream format(String format, Object... args) { return this; }
            @Override public java.io.PrintStream format(java.util.Locale l, String format, Object... args) { return this; }
            @Override public java.io.PrintStream append(CharSequence csq) { return this; }
            @Override public java.io.PrintStream append(CharSequence csq, int start, int end) { return this; }
            @Override public java.io.PrintStream append(char c) { return this; }
        });
    }
}
