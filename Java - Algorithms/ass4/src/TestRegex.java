import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class TestRegex {

    private static final double ROUTER_REGEX_WEIGHT = 0.3;
    private static final double LINK_REGEX_WEIGHT = 0.7;


    public static void main(String[] args) throws IOException {
        List<String[]> routerExpectedGroups = Files.readAllLines(Paths.get(TestUtils.ROUTER_EXPECTED_OUTPUT_FILE)).stream()
                .map(l -> l.split(" ")).collect(Collectors.toList());

        List<String[]> linkExpectedGroups = Files.readAllLines(Paths.get(TestUtils.LINK_EXPECTED_OUTPUT_FILE)).stream()
                .map(l -> l.split(" ")).collect(Collectors.toList());

        Pattern patternRouter = Pattern.compile(Network.routerRegularExpression());
        Pattern patternLink = Pattern.compile(Network.linkRegularExpression());

        List<String> routerInputLines = Files.readAllLines(Paths.get(TestUtils.ROUTER_INPUT_FILE));
        List<String> linkInputLines = Files.readAllLines(Paths.get(TestUtils.LINK_INPUT_FILE));

        double routerScore = testGroups(routerExpectedGroups, patternRouter, routerInputLines);
        double linkScore = testGroups(linkExpectedGroups, patternLink, linkInputLines);

        routerScore /= routerInputLines.size();
        linkScore /= linkInputLines.size();

        System.out.println(routerScore * ROUTER_REGEX_WEIGHT + linkScore * LINK_REGEX_WEIGHT);
    }

    private static double testGroups(List<String[]> expectedGroups, Pattern pattern, List<String> inputLines) {
        double score = 0;

        for (int i = 0; i < inputLines.size(); i++) {
            String line = inputLines.get(i);
            Matcher matcher = pattern.matcher(line);
            if (matcher.find()) {
                if (matcher.groupCount() != expectedGroups.get(i).length) {
                    continue;
                }
                int correct = 0;
                /* If groups are correct */
                for (int j = 0; j < expectedGroups.get(i).length; j++) {
                    if (matcher.group(j + 1).equals(expectedGroups.get(i)[j])) {
                        correct += 1;
                    }
                }
                score += (double) correct / expectedGroups.get(i).length;
            }
        }
        return score;
    }
}
