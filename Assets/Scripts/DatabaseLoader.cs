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

    public static List<PersonajeData> AsignarPersonajes(bool aliados, bool? final)
    {
        List<PersonajeData> listaPersonajes = new List<PersonajeData>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM personajes_principales";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);

                        if (aliados)
                        {
                            if (id == 1 || id == 2 || id == 3 || id == 4)
                            {
                                PersonajeData pj = BuscarPersonaje(reader.GetInt32(0));
                                
                                pj.vida_actual = pj.vida_max;
                                pj.energia_actual = pj.energia_max;
                                pj.FF_actual = pj.FF_base;
                                pj.RF_actual = pj.RF_base;
                                pj.PM_actual = pj.PM_base;
                                pj.RM_actual = pj.RM_base;
                                pj.aggro = 1;
                                pj.enfermo = 0;
                                pj.incapacitado = 0;
                                
                                listaPersonajes.Add(pj);
                            }
                        }
                        else
                        {
                            if (final != null)
                            {
                                if (final == true)
                                {
                                    if (id == 7 || id == 8 || id == 9)
                                    {
                                        PersonajeData pj = BuscarPersonaje(reader.GetInt32(0));
                                
                                        pj.vida_actual = pj.vida_max;
                                        pj.energia_actual = pj.energia_max;
                                        pj.FF_actual = pj.FF_base;
                                        pj.RF_actual = pj.RF_base;
                                        pj.PM_actual = pj.PM_base;
                                        pj.RM_actual = pj.RM_base;
                                        pj.aggro = 1;
                                        pj.enfermo = 0;
                                        pj.incapacitado = 0;
                                
                                        listaPersonajes.Add(pj);
                                    }
                                }
                                else
                                {
                                    if (id == 5 || id == 6)
                                    {
                                        PersonajeData pj = BuscarPersonaje(reader.GetInt32(0));
                                
                                        pj.vida_actual = pj.vida_max;
                                        pj.energia_actual = pj.energia_max;
                                        pj.FF_actual = pj.FF_base;
                                        pj.RF_actual = pj.RF_base;
                                        pj.PM_actual = pj.PM_base;
                                        pj.RM_actual = pj.RM_base;
                                        pj.aggro = 1;
                                        pj.enfermo = 0;
                                        pj.incapacitado = 0;
                                
                                        listaPersonajes.Add(pj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            connection.Close();
        }

        foreach (var pj in listaPersonajes)
        {
            ActualizarPersonaje(pj);
        }

        return listaPersonajes;
    }

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
                command.CommandText = "UPDATE personajes_principales SET" +
                                      " vida_actual = @vida_actual," +
                                      " energia_actual = @energia_actual," +
                                      " FF_actual = @FF_actual," +
                                      " RF_actual = @RF_actual," +
                                      " PM_actual = @PM_actual," +
                                      " RM_actual = @RM_actual," +
                                      " aggro = @aggro," +
                                      " incapacitado = @incapacitado," +
                                      " enfermo = @enfermo" +
                                      " WHERE id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@vida_actual", pj.vida_actual);
                command.Parameters.AddWithValue("@energia_actual", pj.energia_actual);
                command.Parameters.AddWithValue("@FF_actual", pj.FF_actual);
                command.Parameters.AddWithValue("@RF_actual", pj.RF_actual);
                command.Parameters.AddWithValue("@PM_actual", pj.PM_actual);
                command.Parameters.AddWithValue("@RM_actual", pj.RM_actual);
                command.Parameters.AddWithValue("@aggro", pj.aggro);
                command.Parameters.AddWithValue("@incapacitado", pj.incapacitado);
                command.Parameters.AddWithValue("@enfermo", pj.enfermo);
                command.Parameters.AddWithValue("@id", pj.id);

                command.ExecuteNonQuery();
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
                                id = reader.GetInt32(0),
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
                                    cantidad = reader.GetInt32(2),
                                };
                                opciones.Add(consumible);
                            }
                            else
                            {
                                ConsumibleData consumible = new ConsumibleData
                                {
                                    nombre = reader.GetString(1),
                                    cantidad = reader.GetInt32(2),
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