#!/bin/bash

docker build -t tti-model .
docker run -it -v `pwd`/..:/var/tti tti-model