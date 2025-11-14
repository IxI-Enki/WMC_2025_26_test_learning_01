<!-- markdownlint-disable -->

Wir haben bald einen Test im Fach Web-Medien-Computing (WMC) - In diesem Fach haben wir als Testumfang die Clean-Architecture (wie im Directory @CleanArchitecture_Template zu finden ist)

Die zu lernenden Designpattern und Architekturen / Frameworks sind exakt die, die auch in diesem Template implementiert sind, keine anderen. Beim Test dürfen und werden wir auch exakt dieses Template, als Unterstützung, verwenden, ändere absolut NICHTS daran.

Ultrathink für diese gesamte, folgende Aufgabenstellung.

Deine Aufgabe wird jetzt sein mir ein Übungsbeispiel (eine Art Lückentext) in dem Verzeichnis @CleanArchitecture_Uebung_01 zu erstellen, das als Analogie der Angabe, die wir beim Test bekommen werden, dienen soll.
Beim Test werden wir natürlich nicht Sensors/Measurements implementieren müssen, sondern irgendeine andere Software-Aufgabenstellung (mit drei Entitäten - dies wissen wir schon) sei bitte kreativ und denk dir eine Aufgabenstellung aus, die alles abdeckt, aber mich nicht zu sehr überwältigt.
Die Komplexität wird ähnlich der Komplexität des Templates sein.
Wir werden Validierungen auf den drei Ebenen zu implementieren haben -
Fluent Validation bei einer Entität (Validierung), einmal Domain Validation, einmal Application Validation.
Wir werden eine GET ALL, eine GET BY ID, CREATE, eine DELETE und eine UPDATE Methode (Controller) implementieren müssen und für jeden Flow (wie im Template zu sehen Command, CommandHandler, CommandValidator oder das selbe mit Queries und deren Handler und Validator)

(Unittests werden wir auch haben, die uns unterstützen sollten - Auch diese bitte kreieren)

Wie erwähnt soll die Vorlage ein Lückentext sein (implementiere "throw not implemented exceptions" in Controller oder nur die Ordner bei Features z.B.)

und erzeuge eine README als Angabe in @CleanArchitecture_Uebung_01 in der du erklärst welche Validations ich implementieren soll und welche Responses etc.

---

Beachte folge Notizen meiner Kollegin:

Für den Test werden diese Dinge zu machen sein:
Wir müssen Commands mit Validator und Handler etc. selber machen können.
Validations auf Domain- und Application-Ebene
Controller sind zu implementieren
Für Domain-Ebene und API-Ebene wurden Tests erstellt - Methodennamen abgleichen
Man muss bei der Dependency Injection den Service registrieren - also wie ISensorUniquenessChecker

In der Infrastruktur wird DataSeeder und Repositories fertig sein. - müsste normalerweise neu angelegt werden, aber wäre beim Test zu lang.
Repository-Methoden für spezielle Abfragen müssen wir hinzufügen - z. B. erste 100 Messungen, die mit x anfangen.

Bei der API müssen die entsprechenden Controller hinzugefügt werden. 

Ich acker mich so durch, dass ich zuerst die Domain und Infrastruktur aufbaue, bevor ich mich an die API mache. Ich finde, wenn man UniquenessChecker und Validation etc. später erst macht, hat man ja überhaupt keinen Überblick, wo dann nachträglich nochmal was ergänzt werden muss.

🤓🤜🏻🤛🏻🤖

