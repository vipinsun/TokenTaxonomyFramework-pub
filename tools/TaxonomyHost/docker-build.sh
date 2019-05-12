#!/usr/bin/env bash
docker build  -f TaxonomyClient/Dockerfile --rm -t tti/ttf/taxonomyclient .
docker build  -f TaxonomyHost/Dockerfile --rm -t tti/ttf/taxonomyservice .

