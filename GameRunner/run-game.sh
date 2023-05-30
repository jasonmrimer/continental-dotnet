#!/bin/bash

cd ~/workspace

#prep kafka
#cd ~/workspace/confluent-7.3.2/

# Generate a GUID for the session
session_id=$(uuidgen)
# Create Kafka topic names
loop_counter_topic="LoopCounter-$session_id"
loop_counter_response_topic="LoopCounterResponse-$session_id"

# Create Kafka topics
~/workspace/confluent-7.3.2/bin/kafka-topics --create --bootstrap-server localhost:9092 --topic $loop_counter_topic --partitions 1 --replication-factor 1
~/workspace/confluent-7.3.2/bin/kafka-topics --create --bootstrap-server localhost:9092 --topic $loop_counter_response_topic --partitions 1 --replication-factor 1

# Run the .NET app
dotnet run --project ~/workspace/dotnet-kafka-python/GameRunner/GameRunner.csproj --loop-counter-topic $loop_counter_topic --loop-counter-response-topic $loop_counter_response_topic &

#cd ~/workspace/python-kafka-dotnet/src &
# Run the Python app
python ~/workspace/python-kafka-dotnet/src/kafka-counter.py --loop-counter-topic $loop_counter_topic --loop-counter-response-topic $loop_counter_response_topic
