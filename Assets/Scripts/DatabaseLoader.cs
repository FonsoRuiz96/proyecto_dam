using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseLoader : MonoBehaviour
{
    private static string dbPath = "URI=file:" + Application.streamingAssetsPath + "/game.db";

    public static PersonajeData BuscarPersonaje(int pjId)
    {
        PersonajeData pj = null;

        using (SqliteConnection connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM personajes_principales WHERE id = @pjId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@pjId", pjId);
                
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pj = new PersonajeData
                        {
                            id = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            hab_basica_id = reader.GetInt32(2),
                            hab_secundaria_id = reader.GetInt32(3),
                            hab_especial_id = reader.GetInt32(4),
                            usar_objeto_id = reader.GetInt32(5),
                            graficos = reader.GetString(6),
                            vida_max = reader.GetInt32(7),
                            vida_actual = reader.GetInt32(8),
                            energia_max = reader.GetInt32(9),
                            energia_actual = reader.GetInt32(10),
                            FF_base = reader.GetInt32(11),
                            FF_actual = reader.GetInt32(12),
                            RF_base = reader.GetInt32(13),
                            RF_actual = reader.GetInt32(14),
                            PM_base = reader.GetInt32(15),
                            PM_actual = reader.GetInt32(16),
                            RM_base = reader.GetInt32(17),
                            RM_actual = reader.GetInt32(18),
                            agilidad = reader.GetInt32(19),
                            aggro = reader.GetInt32(20),
                            incapacitado = reader.GetInt32(21),
                            enfermo = reader.GetInt32(22),
                            descripcion = reader.GetString(23)
                        };
                    }
                }
            }
            
            connection.Close();
        }
        
        return pj;
    }

    public static void ActualizarPersonaje(PersonajeData pj)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                var props = typeof(PersonajeData).GetProperties();
                List<string> setParts = new List<string>();

                command.Parameters.AddWithValue("@id", pj.id);

                foreach (var prop in props)
                {
                    if (prop.Name == "id") continue;

                    object value = prop.GetValue(pj) ?? DBNull.Value;
                    string paramName = "@" + prop.Name;

                    setParts.Add($"{prop.Name} = {paramName}");
                    command.Parameters.AddWithValue(paramName, value);
                }

                string setClause = string.Join(", ", setParts);
                command.CommandText = $"UPDATE personajes_principales SET {setClause} WHERE id = @id";
            }

            connection.Close();
        }
    }

    public static HabilidadData BuscarHabilidad(int id)
    {
        HabilidadData hab = null;

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM habilidades WHERE id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", id);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hab = new HabilidadData
                        {
                            id = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            cantidad = reader.GetInt32(2),
                            fisica = reader.GetInt32(3),
                            dano = reader.GetInt32(4),
                            vida = reader.GetInt32(5),
                            ofensiva = reader.GetInt32(6),
                            enferma = reader.GetInt32(7),
                            incapacita = reader.GetInt32(8),
                            target_stat = reader.GetString(9),
                            objetivo = reader.GetString(10),
                            coste = reader.GetInt32(11),
                            descripcion = reader.GetString(12)
                        };
                    }
                }
            }
        }

        return hab;
    }

    public static List<HabilidadData> ListaHabilidades(int pjId)
    {
        List<HabilidadData> opciones = new List<HabilidadData>();
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM personajes_principales WHERE id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", pjId);

                int[] idHabilidades = new int[4];

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idHabilidades[0] = reader.GetInt32(2);
                        idHabilidades[1] = reader.GetInt32(3);
                        idHabilidades[2] = reader.GetInt32(4);
                        idHabilidades[3] = reader.GetInt32(5);
                    }
                    else
                    {
                        print("Nada en personajes");
                    }
                }


                for (int i = 0; i < idHabilidades.Length; i++)
                {
                    command.CommandText = "SELECT * FROM habilidades WHERE id = @id";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", idHabilidades[i]);


                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            HabilidadData hab = new HabilidadData
                            {
                                nombre = reader.GetString(1),
                            };
                            opciones.Add(hab);
                        }
                        else
                        {
                            print("Nada en habilidades");
                        }
                    }
                }
            }

            connection.Close();
        }

        return opciones;
    }
    
    public static ConsumibleData BuscarConsumible(int id)
    {
        ConsumibleData consumible = null;

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM consumibles WHERE id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", id);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        consumible = new ConsumibleData
                        {
                            id = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            cantidad = reader.GetInt32(2),
                            vida = reader.GetInt32(3),
                            cantidad_recuperada = reader.GetInt32(4),
                            enfermedad = reader.GetInt32(5),
                            usable_en_combate = reader.GetInt32(6),
                        };
                    }
                }
            }
        }

        return consumible;
    }

    public static List<ConsumibleData> ListaConsumibles(bool combate)
    {
        List<ConsumibleData> opciones = new List<ConsumibleData>();
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM consumibles WHERE usable_en_combate = 1";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(2) > 0)
                        {
                            if (combate)
                            {
                                ConsumibleData consumible = new ConsumibleData
                                {
                                    nombre = reader.GetString(1),
                                };
                                opciones.Add(consumible);
                            }
                            else
                            {
                                ConsumibleData consumible = new ConsumibleData
                                {
                                    nombre = reader.GetString(1),
                                };
                                opciones.Add(consumible);
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        return opciones;
    }

    public static void ActualizarCantidadConsumible(int id, bool aumentar)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                if (aumentar)
                {
                    command.CommandText = "UPDATE consumibles SET cantidad = cantidad + 1 WHERE id = @id";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = "UPDATE consumibles SET cantidad = cantidad - 1 WHERE id = @id";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }
}