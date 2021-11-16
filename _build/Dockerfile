FROM ubuntu:20.04

RUN apt-get update && apt-get install -y \
    gnupg2 \
    wget \
    apt-transport-https \
    nginx \
    systemd

# mongodb
# RUN wget -qO - https://www.mongodb.org/static/pgp/server-4.2.asc | apt-key add -
# RUN sh -c 'echo "deb [ arch=amd64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/4.2 multiverse" | tee /etc/apt/sources.list.d/mongodb-org-4.2.list'

# .net core
# RUN wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
# RUN mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
# RUN wget -q https://packages.microsoft.com/config/debian/9/prod.list
# RUN mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
# RUN chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
# RUN chown root:root /etc/apt/sources.list.d/microsoft-prod.list

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb

RUN apt-get update && apt-get install -y \
    aspnetcore-runtime-3.1 \
    dotnet-sdk-3.1

# COPY mongod.conf /etc/mongod.conf
# RUN mkdir /data
# RUN mkdir /data/db
# COPY mongod /etc/init.d/mongod
# RUN chmod 755 /etc/init.d/mongod

COPY nginx.conf /etc/nginx/nginx.conf

COPY scripts/* /srv/
RUN chmod 755 /srv/*
COPY _package/app /srv/app
WORKDIR /srv
EXPOSE 80

ENV PATH="/srv:${PATH}"
ENV ConnectionStrings__GimPriceParser_ElasticConnection="http://gim-es:9200"
ENV ConnectionStrings__GimPriceParser_MongoConnection="mongodb://admin:7j71pm4fw517vj9@gim-mongo:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false"
ENV MAIL_HOSTNAME="167.172.136.83:2080"

ENTRYPOINT ["entrypoint"]