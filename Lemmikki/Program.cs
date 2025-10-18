using System;
using Microsoft.Data.Sqlite;

class Program
{
    static void InitDb()
    {
        using (var conn = new SqliteConnection("Data Source=pets.db"))
        {
            conn.Open();
            string ownersTable = "CREATE TABLE IF NOT EXISTS owners (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, phone TEXT)";
            string petsTable = "CREATE TABLE IF NOT EXISTS pets (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, species TEXT, owner_id INTEGER)";
            using (var cmd = new SqliteCommand(ownersTable, conn)) cmd.ExecuteNonQuery();
            using (var cmd = new SqliteCommand(petsTable, conn)) cmd.ExecuteNonQuery();
        }
    }

    static void AddOwner(string name, string phone)
    {
        using (var conn = new SqliteConnection("Data Source=pets.db"))
        {
            conn.Open();
            using (var cmd = new SqliteCommand("INSERT INTO owners (name, phone) VALUES (@name, @phone)", conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.ExecuteNonQuery();
            }
        }
    }

    static void AddPet(string name, string species, int ownerId)
    {
        using (var conn = new SqliteConnection("Data Source=pets.db"))
        {
            conn.Open();
            using (var cmd = new SqliteCommand("INSERT INTO pets (name, species, owner_id) VALUES (@name, @species, @ownerId)", conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@species", species);
                cmd.Parameters.AddWithValue("@ownerId", ownerId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    static void UpdateOwnerPhone(int ownerId, string newPhone)
    {
        using (var conn = new SqliteConnection("Data Source=pets.db"))
        {
            conn.Open();
            using (var cmd = new SqliteCommand("UPDATE owners SET phone = @phone WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@phone", newPhone);
                cmd.Parameters.AddWithValue("@id", ownerId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    static string FindOwnerPhoneByPet(string petName)
    {
        using (var conn = new SqliteConnection("Data Source=pets.db"))
        {
            conn.Open();
            using (var cmd = new SqliteCommand("SELECT owners.phone FROM owners JOIN pets ON owners.id = pets.owner_id WHERE pets.name = @petName", conn))
            {
                cmd.Parameters.AddWithValue("@petName", petName);
                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }
    }

    static void Main()
    {
        InitDb();
        while (true)
        {
            Console.WriteLine("1. Lisää omistaja");
            Console.WriteLine("2. Lisää lemmikki");
            Console.WriteLine("3. Päivitä omistajan puhelin");
            Console.WriteLine("4. Etsi puhelin lemmikin nimen perusteella");
            Console.WriteLine("5. Lopeta");
            Console.Write("Valitse toiminto: ");

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("Omistajan nimi: "); string name = Console.ReadLine();
                Console.Write("Omistajan puhelin: "); string phone = Console.ReadLine();
                AddOwner(name, phone);
            }
            else if (choice == "2")
            {
                Console.Write("Lemmikin nimi: "); string name = Console.ReadLine();
                Console.Write("Laji: "); string species = Console.ReadLine();
                Console.Write("Omistajan ID: "); int ownerId = int.Parse(Console.ReadLine());
                AddPet(name, species, ownerId);
            }
            else if (choice == "3")
            {
                Console.Write("Omistajan ID: "); int ownerId = int.Parse(Console.ReadLine());
                Console.Write("Uusi puhelin: "); string newPhone = Console.ReadLine();
                UpdateOwnerPhone(ownerId, newPhone);
            }
            else if (choice == "4")
            {
                Console.Write("Lemmikin nimi: "); string petName = Console.ReadLine();
                string phone = FindOwnerPhoneByPet(petName);
                Console.WriteLine(phone != null ? $"Puhelin: {phone}" : "Ei löytynyt");
            }
            else if (choice == "5") break;
            else Console.WriteLine("Virheellinen valinta");
        }
    }
}