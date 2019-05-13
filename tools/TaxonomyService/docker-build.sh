#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile --rm -t txclient .
docker build -f TaxonomyService/Dockerfile --rm -t tti/ttf/taxonomyservice .
docker-compose up 
