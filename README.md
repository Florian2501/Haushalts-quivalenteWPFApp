# HaushaltsäquivalenteWPFApp

---

WPF App zur Kontrolle der Arbeitsanteile der Mitglieder einer Familie.

### Idee der App

Diese App ist dafür gedacht, die Verteilung der anfallenden Aufgaben im Familienalltag zu dokumentieren. Es wird gespeichert, wer an welchem Tag welche Aufgaben erledigt hat. Anhand dieser Daten werden grafische und tabellarische Auswertungen angeboten, wodurch man anschaulich und einfach ungerechte Verteilungen der Aufgaben erkennen kann. 

Dadurch trägt die App zu mehr Gerechtigkeit im Familienalltag bei und hilft bei der Streitfrage, wer eine noch anstehende Aufgabe erledigen soll. Außerdem ist dank des entstehenden Wettkampfgedanken besonders bei Kindern ein Anstieg der Motivation zu erwarten.

### Prinzipielle Funktionsweise

Zur Inbetriebnahme der App sollten alle daran teilnehmenden Familienmitglieder als Personen erstellt werden. Dazu muss nur der Name im passenden Menü eingegeben werden. Personen können aber natürlich auch noch später hinzugefügt oder wieder entfernt werden.

Außerdem sollten die am häufigsten erfüllten Augaben auch bereits erstellt werden. Auch hier ist es jederzeit möglich neue Aufgaben hinzuzufügen. Dies geht im zugehörigen Menü, wobei ein Name, eine Aufgabenbeschreibung und eine Punktzahl vergeben werden müssen. Der Name sollte kurz und eindeutig sein. Die Beschreibung sollte den genauen Umfang der Aufgabe beinhalten und möglichst wenig Interpretationsspielraum bieten, damit es nicht zu Streit kommen kann, ob die Aufgabe als erfüllt gilt oder nicht. Die Punktzahl muss in Relation zu den anderen Aufgaben stehen, ist aber an sich frei wählbar (Ganzzahl). Empfohlen ist dabei folgendes Vorgehen:

- Gemeinsames Erörtern der denkbar einfachsten Aufgabe, die öfters anfällt
- Vergabe von 2 Punkt für diese "Basisaufgabe"
- Hochrechnung der Schwierigkeit einer neu zu erstellenden Aufgabe im Vergleich zu dieser "Basisaufgabe"
- Berücksichtigung von Zeitaufwand und Voraussetzungen
- Fällt im Verlauf der Nutzung der App doch leichtere Aufgabe als "Basisaufgabe" an, hat man Spielraum mit Wert 1

**Beipiel:** Angenommen man einigt sich auf "Tisch decken" als einfachste Aufgabe. Dann könnte man für die Aufgabe "Essen kochen" beispielsweise 10 Punkte geben, wenn man diese entsprechend als 5 mal so wertvoll ansieht. Hierbei sollte berücksichtigt werden, ob alle Mitglieder diese Aufgabe überhaupt erfüllen können, in diesem Fall, ob alle kochen können/dürfen. Kommt man später zu dem Entschluss, dass beispielsweise "Getränke aus dem Keller holen" noch deutlich einfacher ist als "Tisch decken", kann man für diese Aufgabe problemlos noch 1 Punkt vergeben, ohne alles überarbeiten zu müssen.

Im Normalfall läuft die Bedienung aber so ab, dass man ins passende Menü zum Aufgaben eintragen geht und dort seine erledigten Aufgaben einträgt. Diese werden dann gespeichert und automatisch in die Tabelle eingetragen.

Dort kann man dann die Punktzahlen aller Mitglieder einsehen und den gewünschten Zeitraum der Darstellung wählen. Links im Menü sieht man die Rangliste mit grafischer Unterstützung durch ein Balkendiagramm.

Noch detailliertere Einsichten bekommt man, wenn man in der Tabelle auf die jeweiligen Namen klickt. Dann sieht man in einem neuen Fenster genaue statistische Auswertungen über die bestimmte Person unterstützt von einem Kreisdiagramm. Hier kann man nicht nur die Punktzahlen sehen, sondern auch welche Aufgaben erledigt wurden.

### Aufgabenplanung

Im Kalenderfenster können Aufgaben eingetragen werden, die es in Zukunft zu erledigen gilt. Dabei werden auch vergangene Aufgaben gespeichert, aber nur bis sie 1 Jahr alt sind. Die eingetragenen Aufgaben können mit Start- und Endzeit oder ganztägig eingetragen werden. Außerdem gibt es die Möglichkeit, Aufgaben als wöchentlich zu markieren, sodass sie zur eingegebenen Zeit jede Woche im Kalender erscheinen. Sobald die Aufgabe erledigt ist, kann sie direkt im Kalender als erledigt markeirt werden, wodurch die Punkte in der Tabelle auftauchen.

Der in der App erstellte Kalender kann jederzeit an eine erstellte Mail-Adresse als .ics Datei geschickt werden und so in fast jedes Kalenderprogramm eingebunden werden.

### Dokumentation

[Hier](https://florian2501.github.io/Haushalts-quivalenteWPFApp/) ist die Dokumentation des Projektes verlinkt, welche automatisch mit Doxygen erstellt wurde. Sie wird regelmäßig aktualisiert.
