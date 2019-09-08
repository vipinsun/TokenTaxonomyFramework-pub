#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile --rm -t txclient .
docker build -f TaxonomyService/Dockerfile --rm -t tti/ttf/taxonomyservice .
docker build -f envoy/Dockerfile --rm -t tti/ttf/envoy .
docker build -f ../TTF-Web-UI/Dockerfile --rm -t tti/ttf/ui ../TTF-Web-UI
docker-compose up
