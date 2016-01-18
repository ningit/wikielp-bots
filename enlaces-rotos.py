#!/usr/bin/python3
#
# Bot para generar listas de enlaces rotos (internos y externos)
#
# Require pywikibot: https://www.mediawiki.org/wiki/Manual:Pywikibot

import pywikibot
import urllib.request
import sys

# Establece la conexión
site = pywikibot.Site()

# Explora toda las páginas
todas = site.allpages()

for pag in todas :

	# Explora los enlaces internos de cada página en busca
	# de páginas para completar

	enls = pag.linkedPages()

	for p in enls :
		if not p.exists() :
			print('No existe la página:', p.title())

	# Explora los enlaces externos de cada página en busca
	# de enlaces rotos

	enls = pag.extlinks();

	for url in enls :
		print('Procesando', url, file=sys.stderr)

		try :
			# Se podrían hacer peticiones HEAD por eficiencia
			res = urllib.request.urlopen(url, timeout=10)

		except Exception as e :
			print('Problema con el enlace', url, 'en', pag.title(), '::', e)
