using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using DotNetWikiBot;

/**
 * Clase auxiliar válida para distintos bot que operen sobre la wiki de ELP.
 */
class BotELP : Bot
{

	/**
	 * Registra al bot en la wiki.
	 *
	 * Pide nombre de usuario y contraseña por consola.
	 *
	 * @returns En caso de error devuelve {@code null}.
	 */
	public static Site iniciar()
	{
		// Pide nombre de usuario y contraseña

		Console.Write("Nombre usuario: ");
		string usu = Console.ReadLine();
		Console.Write("Contraseña: ");
		string cña = ReadPassword();
		Console.WriteLine();

		try {
			Site welp = new Site("http://wikis.fdi.ucm.es/ELP/", usu, cña);

			return welp;
		}
		catch (WikiBotException wbe) {
			Console.Error.WriteLine(wbe.Message);

			return null;
		}
	}

	/**
	 * Lee distintas opciones en la línea de comandos.
	 *	-v para modo verboso
	 *	-d para depuración (simulación, no se escriben cambios)
	 */
	public static void leerOpciones(string[] ops)
	{
		if (Array.Exists(ops, "-v".Equals))
			DisableSilenceMode();
		else
			EnableSilenceMode();

		simulado	= Array.Exists(ops, "-d".Equals);
	}

	/**
	 * Lee una contraseña desde la consola.
	 */
	public static string ReadPassword()
	{
		StringBuilder pwd = new StringBuilder();

		while (true)
		{
			ConsoleKeyInfo c = Console.ReadKey(true);

			if (c.Key == ConsoleKey.Enter)
			{
				break;
			}
			else if (c.Key == ConsoleKey.Backspace)
                	{
				if (pwd.Length > 0)
				{
					pwd.Remove(pwd.Length - 1, 1);
					Console.Write("\b \b");
				}
			}
			else
			{
				pwd.Append(c.KeyChar);
				Console.Write("*");
			}
		}

		return pwd.ToString();
	}

	/**
	 * Indica si el bot escribe cambios en la wiki o únicamente lo simula.
	 *
	 * Útil para depuración.
	 */
	protected static bool simulado = false;
}

/**
 * Categoriza los trabajos de la wiki de ELP según el curso académico.
 */
class Categorizador : BotELP
{
	public static int Main()
	{

		// Se registra en la página y configura el bot
		Site welp = iniciar();

		leerOpciones(Environment.GetCommandLineArgs());

		if (welp == null)
			return 1;

		// Cuenta del número de ediciones
		int cuenta = 0;

		// Obtiene todos los trabajos (de momento en el espacio de nombre Principal)
		PageList todas = new PageList(welp);

		todas.FillFromAllPages("Trabajo:", 0, false, Int32.MaxValue, "Trabajo;");

		foreach (Page pag in todas)
		{
			pag.Load();

			// Si ya hay indicación de curso no hace nada
			List<string> cats = pag.GetCategories();

			if (cats.Exists(patronCCurso.IsMatch))
				continue;

			// Para averiguar el curso obtiene la fecha de la
			// primera edición
			PageList hist = new PageList(welp);

			hist.FillFromPageHistory(pag.title, Int32.MaxValue);

			DateTime fc = hist[hist.Count()-1].timestamp;

			// Distingue en base a ella el curso
			int año = fc.Year;

			// Si es antes del 29 de septiembre (aprox) es que es
			// del curso que empieza en el año anterior
			if (fc.Month < 9 || fc.Month == 9 && fc.Day < 29)
				año--;

			string curso = "Curso " + año + "-" + (año + 1);

			// Muestra información por consola
			Console.Error.WriteLine("«" + pag.title + "» creado en " + fc + " => " + curso);

			cuenta++;

			if (!simulado) {
				pag.AddToCategory(curso);

				pag.Save("bot: categorización de trabajos por curso", true);
			}
		}

		// Resumen de las operaciones
		Console.Error.WriteLine("El bot " + (simulado ? "hubiera realizado " : "realizó ") + cuenta + " ediciones.");

		return 0;
	}

	/**
	 * Patrón en el que encajan las categorías de curso académico.
	 */
	private static Regex patronCCurso = new Regex("^Categoría:Curso \\d+-\\d+$", RegexOptions.Compiled);
}
