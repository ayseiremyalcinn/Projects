import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;

public class Main {

    public static void main(String[] args) throws IOException {

        String input0 = args[0];            // get input
        String input1 = args[1];
        String input2 = args[2];
        String output = args[3];

        ArrayList<String> people = readFile(input0);            // read input files
        ArrayList<String> films = readFile(input1);
        ArrayList<String> commands = readFile(input2);

        BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(output, true), StandardCharsets.UTF_8));


        //................ MAKE PEOPLE OBJECTS LIST WHICH INCLUDE PEOPLE.................

        ArrayList<Person> peopleObjects = new ArrayList<>();
        ArrayList<User> userObjects = new ArrayList<>();
        ArrayList<Director> directorObjects = new ArrayList<>();
        ArrayList<Writer> writerObjects = new ArrayList<>();
        ArrayList<Actor> actorObjects = new ArrayList<>();
        ArrayList<ChildActor> childActorObjects = new ArrayList<>();
        ArrayList<StuntPerformer> stuntPerformerObjects = new ArrayList<>();
        ArrayList<String> ids = new ArrayList<>();

        for (String person: people) {
            String[] personList = person.split("\t");

            switch (personList[0]) {
                case "Actor:" :
                    Actor actor = new Actor(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4], Integer.parseInt(personList[5]));
                    peopleObjects.add(actor);
                    actorObjects.add(actor);
                    ids.add(personList[1]);
                    break;

                case "Director:" :
                    Director director = new Director(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4], personList[5]);
                    peopleObjects.add(director);
                    directorObjects.add(director);
                    ids.add(personList[1]);
                    break;

                case "Writer:" :
                    Writer writerr = new Writer(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4], personList[5]);
                    peopleObjects.add(writerr);
                    writerObjects.add(writerr);
                    ids.add(personList[1]);
                    break;

                case "ChildActor:" :
                    ChildActor childActor = new ChildActor(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4], Integer.parseInt(personList[5]));
                    peopleObjects.add(childActor);
                    childActorObjects.add(childActor);
                    ids.add(personList[1]);
                    break;

                case "StuntPerformer:" :
                    String[] temp = personList[6].split(",");
                    int[] tmp = new int[temp.length];
                    for (int i = 0; i< temp.length; i++) {
                        tmp[i] = Integer.parseInt(temp[i]);
                    }
                    StuntPerformer stuntPerformer = new StuntPerformer(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4], Integer.parseInt(personList[5]),tmp);
                    peopleObjects.add(stuntPerformer);
                    stuntPerformerObjects.add(stuntPerformer);
                    ids.add(personList[1]);
                    break;

                case "User:" :
                    User user = new User(Integer.parseInt(personList[1]), personList[2], personList[3], personList[4]);
                    peopleObjects.add(user);
                    userObjects.add(user);
                    ids.add(personList[1]);
                    break;

            }

        }

        // ................MAKE LIST OF FILM OBJECTS..................................

        ArrayList<Film> filmsObjects = new ArrayList<>();
        ArrayList<ShortFilm> shortFilmObjects = new ArrayList<>();
        ArrayList<Documantaries> documantaryObjects = new ArrayList<>();
        ArrayList<TvSeries> tvSeriesObjects = new ArrayList<>();
        ArrayList<FeatureFilm> featureFilmObjects = new ArrayList<>();

        for (String film: films) {
            String[] filmList = film.split("\t");

            String[] temp0 = filmList[4].split(",");
            int[] tmp0 = new int[temp0.length];
            for (int i = 0; i< temp0.length; i++) {
                tmp0[i] = Integer.parseInt(temp0[i]);
            }

            String[] temp1 = filmList[7].split(",");
            int[] tmp1 = new int[temp1.length];
            for (int i = 0; i< temp1.length; i++) {
                tmp1[i] = Integer.parseInt(temp1[i]);
            }

            switch (filmList[0]){
                case "FeatureFilm:" :
                    String[] temp2 = filmList[10].split(",");
                    int[] tmp2 = new int[temp2.length];
                    for (int i = 0; i< temp2.length; i++) {
                        tmp2[i] = Integer.parseInt(temp2[i]);
                    }

                    FeatureFilm featureFilm = new FeatureFilm(Integer.parseInt(filmList[1]), filmList[2], filmList[3], tmp0, Integer.parseInt(filmList[5]), filmList[6], tmp1, filmList[8].split(","), filmList[9],tmp2, Integer.parseInt(filmList[11]) );
                    filmsObjects.add(featureFilm);
                    featureFilmObjects.add(featureFilm);
                    break;

                case "ShortFilm:" :
                    String[] temp_ = filmList[10].split(",");
                    int[] tmp_ = new int[temp_.length];
                    for (int i = 0; i< temp_.length; i++) {
                        tmp_[i] = Integer.parseInt(temp_[i]);
                    }

                    if(Integer.parseInt(filmList[5]) <= 40){
                        ShortFilm shortFilm = new ShortFilm(Integer.parseInt(filmList[1]), filmList[2], filmList[3], tmp0, Integer.parseInt(filmList[5]), filmList[6], tmp1, filmList[8].split(","), filmList[9],tmp_);
                        filmsObjects.add(shortFilm);
                        shortFilmObjects.add(shortFilm);
                    }else{
                        System.out.println("There is a invalid shortFilm in films.txt file");
                    }
                    break;

                case "Documentary:" :
                    Documantaries documantary = new Documantaries(Integer.parseInt(filmList[1]), filmList[2], filmList[3], tmp0, Integer.parseInt(filmList[5]), filmList[6], tmp1, filmList[8]);
                    filmsObjects.add(documantary);
                    documantaryObjects.add(documantary);
                    break;

                case "TVSeries:" :
                    String[] temp3 = filmList[9].split(",");
                    int[] tmp3 = new int[temp3.length];
                    for (int i = 0; i< temp3.length; i++) {
                        tmp3[i] = Integer.parseInt(temp3[i]);
                    }

                    TvSeries tvSeries = new TvSeries(Integer.parseInt(filmList[1]), filmList[2], filmList[3], tmp0, Integer.parseInt(filmList[5]), filmList[6], tmp1, filmList[8].split(","), tmp3, filmList[10], filmList[11], Integer.parseInt(filmList[12]),Integer.parseInt(filmList[13]));
                    filmsObjects.add(tvSeries);
                    tvSeriesObjects.add(tvSeries);
                    break;

            }
        }

        //............................DO COMMANDS..............................

        for(String command: commands){
            String[] commandList = command.split("\t");
            writer.write(command + "\n\n");
            switch (commandList[0]) {
                case "RATE" :
                    boolean myBool = true;
                    User user = findPerson(Integer.parseInt(commandList[1]), userObjects);        // FIND USER WHO HAS THE SAME ID
                    Film film0 = findFilm(Integer.parseInt(commandList[2]), filmsObjects);
                    if(user == null || film0 == null){
                        writer.write("Command Failed\n" + "User ID: " + commandList[1] + "\nFilm ID: " + commandList[2] );             //CHECK EXIST OR NOT
                    }else{
                        for (Film film : filmsObjects) {
                            if (user.getRates() != null && film.getId() == Integer.parseInt(commandList[2])) {
                                for (String i : user.getRates()) {
                                    if (i.contains(String.valueOf(film.getId()))) {     // CHECK IS THERE SAME FILM
                                        myBool = false;
                                        break;
                                    }
                                }
                            }
                            if (!myBool) {                        // CHECK IS THERE SAME FILM
                                writer.write("This film was earlier rated");
                                break;
                            } else if (Integer.parseInt(commandList[2]) == film.getId() && myBool) {
                                user.addRate(Integer.parseInt(commandList[2]), Integer.parseInt(commandList[3]));
                                writer.write("Film rated successfully\n" + "Film type: " + film.getClass().getName() + "\nFilm title: " + film.getTitle());
                                break;

                            }
                        }
                    }
                    break;
                case "ADD" :
                    boolean exist = false;
                    boolean myBool0 = false;
                    boolean myBool1 = false;
                    boolean myBool2 = false;
                    for (FeatureFilm film : featureFilmObjects) {
                        if (film.getId() == Integer.parseInt(commandList[2])) {
                            exist = true;
                            break;
                        }
                    }
                    String[] temp0 = commandList[5].split(",");
                    int[] directors = new int[temp0.length];
                    for (int i = 0; i < temp0.length; i++) {
                        directors[i] = Integer.parseInt(temp0[i]);
                    }
                    String[] temp1 = commandList[8].split(",");
                    int[] performers = new int[temp1.length];
                    for (int i = 0; i < temp1.length; i++) {
                        performers[i] = Integer.parseInt(temp1[i]);
                    }
                    String[] temp2 = commandList[11].split(",");
                    int[] writers = new int[temp2.length];
                    for (int i = 0; i < temp2.length; i++) {                                      //     CHECK DIRECTOR, PERFORMER AND WRITER EXIST.
                        writers[i] = Integer.parseInt(temp2[i]);
                    }
                    for (int director : directors) {
                        if (ids.contains(String.valueOf(director))) {
                            myBool0 = true ;
                        } else {
                            myBool0 = false;
                            break;
                        }
                    }
                    for (int performer : performers) {
                        if (ids.contains(String.valueOf(performer))) {
                            myBool1 = true;
                        } else {
                            myBool1 = false;
                            break;
                        }
                    }
                    for (int w : writers) {
                        if (ids.contains(String.valueOf(w))) {
                            myBool2 = true;
                        } else {
                            myBool2 = false;
                            break;
                        }
                    }
                    if (!exist && myBool0 && myBool1 && myBool2) {
                        writer.write("FeatureFilm added successfully\n" + "Film ID: " + commandList[2] + "\nFilm title: " + commandList[3]);

                        FeatureFilm featureFilm = new FeatureFilm(Integer.parseInt(commandList[2]), commandList[3], commandList[4], directors, Integer.parseInt(commandList[6]), commandList[7], performers, commandList[9].split(","), commandList[10], writers, Integer.parseInt(commandList[12]));
                        featureFilmObjects.add(featureFilm);
                        filmsObjects.add(featureFilm);
                    } else {
                        writer.write( "Command Failed\n" + "Film ID: " + commandList[2] + "\nFilm title: " + commandList[3]);
                    }
                    break;
                case "VIEWFILM" :
                    Film film = findFilm(Integer.parseInt(commandList[1]), filmsObjects);  // FIND FILM WHICH IS THE SAME ID.
                    if (film == null) {
                        writer.write("Command Failed\n" + "Film ID:" + commandList[1]);   // CHECK EXIST OR NOT.
                    } else if (film.getClass().getName().equals("FeatureFilm") || film.getClass().getName().equals("ShortFilm")) {
                        if (film.getClass().getName().equals("FeatureFilm")) {
                            FeatureFilm obje = findFilm1(Integer.parseInt(commandList[1]), featureFilmObjects);   // FIND FILM WHICH IS THE SAME ID.
                            writer.write( obje.getTitle() + "(" + obje.getReleaseDate().split("\\.")[2] + ")\n\n" + Arrays.toString(obje.getGenre()) + "\n" + "Writers:" + findWriters(obje, writerObjects) + "\nDirectors:" + findDirectors(obje, directorObjects) + "\nStars:" + findStars(obje, peopleObjects) + "\n");
                            writer.write(findRate(obje, userObjects, commandList[0]));

                        } else {
                            ShortFilm obje = findFilm1(Integer.parseInt(commandList[1]), shortFilmObjects);   // FIND FILM WHICH IS THE SAME ID.
                            writer.write( obje.getTitle() + "(" + obje.getReleaseDate().split("\\.")[2] + ")\n\n" + Arrays.toString(obje.getGenre()) + "\n" + "Writers:" + findWriters(obje, writerObjects) + "\nDirectors:" + findDirectors(obje, directorObjects) + "\nStars:" + findStars(obje, peopleObjects) + "\n");
                            writer.write(findRate(obje, userObjects, commandList[0]));
                        }
                    } else if (film.getClass().getName().equals("Documantaries")) {
                        Documantaries obje = findFilm1(Integer.parseInt(commandList[1]), documantaryObjects);    // FIND FILM WHICH IS THE SAME ID.
                        writer.write(obje.getTitle() + "(" + obje.getReleaseDate().split("\\.")[2] + ")" + "\n\nDirectors:" + findDirectors(obje, directorObjects) + "\nStars:" + findStars(obje, peopleObjects) + "\n");
                        writer.write(findRate(obje, userObjects, commandList[0]));
                    } else if (film.getClass().getName().equals("TvSeries")) {
                        TvSeries obje = findFilm1(Integer.parseInt(commandList[1]), tvSeriesObjects);    // FIND FILM WHICH IS THE SAME ID.
                        writer.write(obje.getTitle() + " " + "(" + obje.getStartDate().split("\\.")[2] + "-" + obje.getEndDate().split("\\.")[2] + ")\n\n" + obje.getSeason() + " seasons " + obje.getEpisode() + " episodes\n" + Arrays.toString(obje.getGenre()) + "\n" + "Writers:" + findWriters(obje, writerObjects) + "\nDirectors:" + findDirectors(obje, directorObjects) + "\nStars:" + findStars(obje, peopleObjects) + "\n");
                        writer.write(findRate(obje, userObjects, commandList[0]));
                    }
                    break;
                case "LIST" :
                    switch (commandList[1]) {
                        case "USER" :
                            User userr = findPerson(Integer.parseInt(commandList[2]), userObjects);   // FIND USER WHO IS THE SAME ID.
                            if (userr == null) {
                                writer.write("Command Failed\n" + "User ID:" + commandList[2] );    // CHECK EXIST OR NOT.
                            } else {
                                if (userr.getRates() == null) {
                                    writer.write( "There is not any ratings so far");
                                } else {
                                    for (String rate : userr.getRates()) {
                                        String[] tmp = rate.split(":");
                                        Film filmm = findFilm(Integer.parseInt(tmp[0]), filmsObjects);
                                        writer.write(filmm.getTitle() + ":" + tmp[1] + "\n");
                                    }
                                }
                            }
                            break;
                        case "FILM" :
                            if (tvSeriesObjects.size() == 0) {      // CHECK THERE IS ANY TV SERIES.
                                writer.write("No result\n");
                            } else {
                                for (TvSeries series : tvSeriesObjects) {        // GET ALL TV SERIES.
                                    writer.write(series.getTitle() + " " + "(" + series.getStartDate().split("\\.")[2] + "-" + series.getEndDate().split("\\.")[2] + ")\n" + series.getSeason() + " seasons and " + series.getEpisode() + " episodes\n\n");
                                }
                            }
                            break;
                        case "FILMS" :
                            if (commandList[3].equals("COUNTRY")) {
                                boolean myBoolean = false;
                                for (Film filmmm : filmsObjects) {
                                    if (filmmm.getCountry().equals(commandList[4])) {
                                        myBoolean = true;
                                        writer.write("Film title: " + filmmm.getTitle() + "\n" + filmmm.getLength() + " min\nLanguage: " + filmmm.getLanguage() + "\n\n");
                                    }
                                }
                                if (!myBoolean) {                       // CHECK EXIST OR NOT.
                                    writer.write("No result");
                                }
                            } else if (commandList[3].equals("RATE")) {
                                ArrayList<String> featureList = new ArrayList<>();
                                for (FeatureFilm film1 : featureFilmObjects) {
                                    String featureString = findRate(film1, userObjects, commandList[0]) + "|" + film1.getTitle() + " (" + film1.getReleaseDate().split("\\.")[2] + ") " + findRate(film1, userObjects, commandList[0]);
                                    featureList.add(featureString);
                                }
                                if (featureFilmObjects.size() == 0) {
                                    writer.write("No result");
                                } else {
                                    Collections.sort(featureList);
                                    writer.write("\nFeatureFilm:\n");
                                    for (int i = featureList.size() - 1; i >= 0; i--) {
                                        int a = featureList.get(i).indexOf("|");
                                        writer.write(featureList.get(i).substring(a + 1) + "\n");
                                    }
                                }

                                ArrayList<String> shortList = new ArrayList<>();
                                for (ShortFilm film2 : shortFilmObjects) {
                                    String shortString = findRate(film2, userObjects, commandList[0]) + "|" + film2.getTitle() + " (" + film2.getReleaseDate().split("\\.")[2] + ") " + findRate(film2, userObjects, commandList[0]);
                                    shortList.add(shortString);
                                }
                                if (shortFilmObjects.size() == 0) {
                                    writer.write("No result");
                                } else {
                                    Collections.sort(shortList);
                                    writer.write("\nShortFilm:\n");
                                    for (int i = shortList.size() - 1; i >= 0; i--) {
                                        int a = shortList.get(i).indexOf("|");
                                        writer.write(shortList.get(i).substring(a + 1) + "\n");
                                    }
                                }

                                ArrayList<String> documanList = new ArrayList<>();
                                for (Documantaries film3 : documantaryObjects) {
                                    String documanString = findRate(film3, userObjects, commandList[0]) + "|" + film3.getTitle() + " (" + film3.getReleaseDate().split("\\.")[2] + ") " + findRate(film3, userObjects, commandList[0]);
                                    documanList.add(documanString);
                                }
                                if (documantaryObjects.size() == 0) {
                                    writer.write("No result");
                                } else {
                                    Collections.sort(documanList);
                                    writer.write("\nDocumentary:\n");
                                    for (int i = documanList.size() - 1; i >= 0; i--) {
                                        int a = documanList.get(i).indexOf("|");
                                        writer.write(documanList.get(i).substring(a + 1) + "\n");
                                    }
                                }

                                ArrayList<String> tvList = new ArrayList<>();
                                for (TvSeries film4 : tvSeriesObjects) {
                                    String tvString = findRate(film4, userObjects, commandList[0]) + "|" + film4.getTitle() + " (" + film4.getStartDate().split("\\.")[2] + "-" + film4.getEndDate().split("\\.")[2] + ") " + findRate(film4, userObjects, commandList[0]);
                                    tvList.add(tvString);
                                }
                                if (tvSeriesObjects.size() == 0) {
                                    writer.write("No result");
                                } else {
                                    Collections.sort(tvList);
                                    writer.write("\nTVSeries:\n");
                                    for (int i = tvList.size() - 1; i >= 0; i--) {
                                        int a = tvList.get(i).indexOf("|");
                                        writer.write(tvList.get(i).substring(a + 1) + "\n");
                                    }
                                }
                            }
                            break;
                        case "FEATUREFILMS" :
                            boolean myBooll = false;
                            for (FeatureFilm film5 : featureFilmObjects) {
                                if (commandList[2].equals("BEFORE")) {
                                    if (Integer.parseInt(film5.getReleaseDate().split("\\.")[2]) < Integer.parseInt(commandList[3])) {
                                        myBooll = true;
                                        writer.write("Film title: " + film5.getTitle() + " (" + film5.getReleaseDate().split("\\.")[2] + ")\n" + film5.getLength() + " min\nLanguage: " + film5.getLanguage() + "\n\n");
                                    }
                                } else if (commandList[2].equals("AFTER")) {
                                    if (Integer.parseInt(film5.getReleaseDate().split("\\.")[2]) >= Integer.parseInt(commandList[3])) {
                                        myBooll = true;
                                        writer.write("Film title: " + film5.getTitle() + " (" + film5.getReleaseDate().split("\\.")[2] + ")\n" + film5.getLength() + " min\nLanguage: " + film5.getLanguage()+ "\n\n" );
                                    }
                                }
                            }
                            if (!myBooll) {
                                writer.write("No result");
                            }
                            break;
                        case "ARTISTS" :
                            boolean bool0 = false, bool1 = false, bool2 = false, bool3 = false, bool4 = false;
                            writer.write("\nDirectors: \n");
                            for (Director direc : directorObjects) {
                                if (direc.getCountry().equals(commandList[3])) {
                                    bool0 = true;
                                    writer.write(direc.getName() + " " + direc.getSurname() + " " + direc.getAgent() + "\n");
                                }
                            }
                            if (!bool0) {
                                writer.write("No result\n");
                            }
                            writer.write("\nWriters: \n");
                            for (Writer w : writerObjects) {
                                if (w.getCountry().equals(commandList[3])) {
                                    bool1 = true;
                                    writer.write(w.getName() + " " + w.getSurname() + " " + w.getStyle() + "\n");
                                }
                            }
                            if (!bool1) {
                                writer.write("No result\n");
                            }
                            writer.write("\nActors: \n");
                            for (Actor actor : actorObjects) {
                                if (actor.getCountry().equals(commandList[3])) {
                                    bool2 = true;
                                    writer.write(actor.getName() + " " + actor.getSurname() + " " + actor.getHeight() + " cm\n");
                                }
                            }
                            if (!bool2) {
                                writer.write("No result\n");
                            }
                            writer.write("\nChildActors: \n");
                            for (ChildActor childActor : childActorObjects) {
                                if (childActor.getCountry().equals(commandList[3])) {
                                    bool3 = true;
                                    writer.write(childActor.getName() + " " + childActor.getSurname() + " " + childActor.getAge() + "\n");
                                }
                            }
                            if (!bool3) {
                                writer.write("No result\n");
                            }
                            writer.write("\nStuntPerformers: \n");
                            for (StuntPerformer stuntPerformer : stuntPerformerObjects) {
                                if (stuntPerformer.getCountry().equals(commandList[3])) {
                                    bool4 = true;
                                    writer.write(stuntPerformer.getName() + " " + stuntPerformer.getSurname() + " " + stuntPerformer.getHeight() + " cm\n");
                                }
                            }
                            if (!bool4) {
                                writer.write("No result");
                            }
                            break;
                    }
                    break;
                case "EDIT":
                case "REMOVE" :
                    boolean myboolll = false;
                    User userrr = findPerson(Integer.parseInt(commandList[2]), userObjects);
                    Film filmmm = findFilm(Integer.parseInt(commandList[3]), filmsObjects);
                    if (userrr == null || userrr.getRates().size() == 0 || filmmm == null) {
                        writer.write("Command Failed\n" + "User ID:" + commandList[2] + "\nFilm ID:" + commandList[3] );
                    } else {
                        for (String rate : userrr.getRates()) {
                            String[] tmp = rate.split(":");
                            if (tmp[0].equals(commandList[3])) {
                                myboolll = true;
                                userrr.removeRate(Integer.parseInt(commandList[3]));
                                if (commandList[0].equals("EDIT")) {
                                    userrr.addRate(Integer.parseInt(commandList[3]), Integer.parseInt(commandList[4]));
                                    writer.write("New ratings done successfully\nFilm title:" + filmmm.getTitle() + "\nYour rating:" + commandList[4]);
                                } else {
                                    writer.write("Your film rating was removed successfully\nFilm title:" + filmmm.getTitle());
                                }
                                break;
                            }
                        }
                        if (!myboolll) {
                            writer.write("Command Failed\n" + "User ID:" + commandList[2] + "\nFilm ID:" + commandList[3] );
                        }
                    }
                    break;
            }
            writer.write("\n\n-----------------------------------------------------------------------------------------------------\n");
        } writer.close();
    }




    //..............................................FUNCTIONS.........................................

    public static ArrayList<String> readFile(String input) throws IOException {

        BufferedReader br = new BufferedReader(new FileReader(input));
        ArrayList<String> file = new ArrayList<>();
        while (true) {
            String data = br.readLine();
            if (data != null) {
                file.add(data);
            } else {
                break;
            }
        }
        return file;
    }
    public static Film findFilm(int id, ArrayList<Film> file){
        for(Film film: file){
            if (film.getId() == id){
                return film;
            }
        }
        return null;
    }
    public static User findPerson(int id, ArrayList<User> file){
        for(User user: file){
            if (user.getId() == id){
                return user;
            }
        }
        return null;
    }

    public static <T extends Film> T findFilm1(int id, ArrayList<T> file){
        for(T film: file){
            if (film.getId() == id){
                return film;
            }
        }
        return null;
    }
    public static String findWriters(FeatureFilm obje, ArrayList<Writer> list){
        String result = "";
        for(int writer: obje.getWriters()){
            for(Writer w: list){
                if (w.getId() == writer){
                    result = result + " " + w.getName() + " " + w.getSurname() + ",";
                }
            }
        }
        return result;
    }
    public static String findWriters(ShortFilm obje, ArrayList<Writer> list){
        String result = "";
        for(int writer: obje.getWriters()){
            for(Writer w: list){
                if (w.getId() == writer){
                    result = result + " " + w.getName() + " " + w.getSurname() + ",";
                }
            }
        }
        return result;
    }
    public static String findWriters(TvSeries obje, ArrayList<Writer> list){
        String result = "";
        for(int writer: obje.getWriters()){
            for(Writer w: list){
                if (w.getId() == writer){
                    result = result + " " + w.getName() + " " + w.getSurname() + ",";
                }
            }
        }
        return result;
    }
    public static <T extends Film> String findDirectors(T obje, ArrayList<Director> list){
        String result = "";
        for(int director: obje.getDirectors()){
            for(Director d: list){
                if (d.getId() == director){
                    result = result + " " + d.getName() + " " + d.getSurname() + ",";
                }
            }
        }
        return result;
    }
    public static <T extends Film> String findStars(T obje, ArrayList<Person> list){
        String result = "";
        for(int star: obje.getPerformers()){
            for(Person p: list){
                if (p.getId() == star){
                    result = result + " " + p.getName() + " " + p.getSurname() + ",";
                }
            }
        }
        return result;
    }

    public static String findRate(Film obje, ArrayList<User> list, String command){
        double result = 0;
        int sayac = 0;
        for(User user: list){
            for(String rate: user.getRates()){
                if(rate.contains(String.valueOf(obje.getId()))){
                    result += Integer.parseInt(rate.split(":")[1]);
                    sayac ++;
                    break;
                }
            }
        }
        if(sayac == 0){
            if(command.equals("LIST")){
                return "Ratings: " +  sayac + "/10 from " + sayac + " users";
            }
            else if(command.equals("VIEWFILM")){
                return "Awaiting for votes";
            }
        }

        return "Ratings: " + String.format("%.1f", result / sayac  )+ "/10 from " + sayac + " users";
    }
}
