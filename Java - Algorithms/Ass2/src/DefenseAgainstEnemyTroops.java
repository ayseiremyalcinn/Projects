import java.util.ArrayList;

/**
 * This class accomplishes Mission Nuke'm
 */
public class DefenseAgainstEnemyTroops {
    private ArrayList<Integer> numberOfEnemiesArrivingPerHour;

    public DefenseAgainstEnemyTroops(ArrayList<Integer> numberOfEnemiesArrivingPerHour){
        this.numberOfEnemiesArrivingPerHour = numberOfEnemiesArrivingPerHour;
    }

    public ArrayList<Integer> getNumberOfEnemiesArrivingPerHour() {
        return numberOfEnemiesArrivingPerHour;
    }

    private int getRechargedWeaponPower(int hoursCharging){
        return hoursCharging*hoursCharging;
    }

    /**
     *     Function to implement the given dynamic programming algorithm
     *     SOL(0) <- 0
     *     HOURS(0) <- [ ]
     *     For{j <- 1...N}
     *         SOL(j) <- max_{0<=i<j} [ (SOL(i) + min[ E(j), P(j − i) ] ]
     *         HOURS(j) <- [HOURS(i), j]
     *     EndFor
     *
     * @return OptimalEnemyDefenseSolution
     */
    public OptimalEnemyDefenseSolution getOptimalDefenseSolutionDP(){
        int N = numberOfEnemiesArrivingPerHour.size();
        int[] sol = new int[N];
        sol[0] = 0;
        ArrayList[] hours = new ArrayList[N];
        hours[0] = new ArrayList<>();
        for (int j = 0; j < N; j++) {
            int max = 0;
            int maxi = 0;
            for (int i = 0; i < j; i++) {
                int temp = sol[i] + Integer.min(numberOfEnemiesArrivingPerHour.get(j), getRechargedWeaponPower(j-1));
                if(temp > max){
                    max = temp;
                    maxi = i;
                }
            }
            sol[j] = max;
            hours[j] = hours[maxi];
            hours[j].add(j);
        }
        return null;
    }
}
