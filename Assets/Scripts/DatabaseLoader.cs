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
                                id = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                cantidad = reader.GetInt32(2),
                                fisica = reader.GetInt32(3),
                                dano = reader.GetInt32(4),
                                vida = reader.GetInt32(5),
                                ofensiva = reader.GetInt32(6),
                                enferma = reader.GetInt32(7),
                                target_stat = reader.GetString(8),
                                objetivo = reader.GetString(9),
                                coste = reader.GetInt32(10),
                                descripcion = reader.GetString(11)
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
                                    id = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    cantidad = reader.GetInt32(2),
                                    vida = reader.GetInt32(3),
                                    cantidad_recuperada = reader.GetInt32(4),
                                    enfermedad = reader.GetInt32(5),
                                };
                                opciones.Add(consumible);
                            }
                            else
                            {
                                ConsumibleData consumible = new ConsumibleData
                                {
                                    id = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    cantidad = reader.GetInt32(2),
                                    vida = reader.GetInt32(3),
                                    cantidad_recuperada = reader.GetInt32(4),
                                    enfermedad = reader.GetInt32(5),
                                    usable_en_combate = reader.GetInt32(6),
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