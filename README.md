## Start Zookeeper
bin/zookeeper-server-start.sh config/zookeeper.properties

## Start Kafka Servers (3 Nodes)
bin/kafka-server-start.sh config/server.properties\
bin/kafka-server-start.sh config/server2.properties\
bin/kafka-server-start.sh config/server3.properties

## Start Kafka Servers (3 Nodes)
bin/kafka-topics.sh --create \
--zookeeper localhost:2181 \
--replication-factor 3 \
--partitions 3 \
--topic LOOP-TOPIC

## Display Partitions and Replicas
bin/kafka-topics.sh --describe --zookeeper localhost:2181 --topic LOOP-TOPIC

## Delete a Topic if Needed
bin/kafka-topics.sh --delete --zookeeper localhost:2181 --topic LOOP-TOPIC

## Sample
https://medium.com/@iet.vijay/kafka-multi-brokers-multi-consumers-and-message-ordering-b61ad7841875
