# Einführung
MMFT ist ein Peer To Peer Messenger mit lokaler, dezentraler Datenbank. MMFT ermöglicht es, Nachrichten und Dateien zwischen Geräten innerhalb eines Netzwerkes zu versenden.
Entwickelt wurde MMFT in C# mit .net10, einer SQLite Datenbank und versendet durch TCP Json Pakete, der Inhalt wird dabei mittels RSA verschlüsselt.
Geschrieben und entwickelt wurde MMFT von Maren Wenz, Milena Wagner, Franz Horatio Ingo Hirsch und Tim Felix Klimmer.

# Arbeitsaufteilung:
Maren Wenz:
Datenbankschema
GUI Entwurf First Time Log In und regulärer Log In
GUI implmentierung First Time Log In und regulärer Log In (FirstAccessLogin.xaml|Login.xaml)

Franz Horatio Ingo Hirsch:
GUI Entwurf Chat Fenster
Dokumentation
Organisation von Aufgaben und Arbeitsaufteilung

Milena Wagner:
TCP Kommunikation

Tim Felix Klimmer:
Datenbank
RSA Verschlüsselung


# Technische Details:
es werden TCP Pakete mit Json Daten versendet. Die Datenbank ist eine SQLite Datenbank, die lokal auf dem Gerät gespeichert wird, und Einträge werden durch RSA verschlüsselt.

Datenbank:
Es gibt die folgenden Tabellen:
P_Nutzer, Nutzer und Nachrichten.

P_Nutzer verweist auf die Tabelle Nutzer mit der UUID des Clients und speichert den privaten RSA Schlüssel des Clients, sowie ein fortlaufender Identifier.

Nutzer speichert die UUID, den öffentlichen RSA Schlüssel und den Benutzernamen von Kontakten, sowie das Profilbild und die Statusmeldung eines Kontaktes. Der erste Kontakt dieser Tabelle ist der eigene Client.

Nachrichten speichert die UUID des Empfängers, die UUID des Senders, den Zeitstempel und dann den Textinhalt sowie Dateiinhalt der Nachricht. Der Inhalt der Nachricht wird dabei mittels RSA verschlüsselt und kann leer sein.

Zudem wird die Datenbank als lokale Datei gespeichert, beim ersten Verwenden des Client erstellt der Client die Datenbankdatei, falls diese nicht vorhanden ist. 
Die Datei wird zur Überprüfung verwendet, ob der Client bereits einmal gestartet wurde, und ob die Datenbank bereits initialisiert wurde, und gibt dem Client die Möglichkeit, die Datenbank zu initialisieren, falls diese noch nicht initialisiert wurde.
Dabei wird der FirstAccessLogin Screen verwendet.


TCP:
Es werden die folgenden Ports verwendet:
50001
50002
50003
Und die folgenden Ports als Backup:
60001
60002
60003


RSA: