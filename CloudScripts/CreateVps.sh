#! /bin/bash

function CreateManyClientVps() {
  local BUILD_NAME=$1
  local REGION=$2
  local SERVER_IP=$3
  local I1=$4
  local I2=$5

  CreateStartupScript client $BUILD_NAME $SERVER_IP

  for ((i=$I1;i<=$I2;i++))  do
    local VPS_NAME=$CLIENT_BASE_NAME$i
    
    CreateClientVps $VPS_NAME $REGION
  done  
}

function CreateClientVps() {
  local VPS_NAME=$1
  local REGION=$2

  if ($RUN_CLOUD) then
    gcloud compute instances create $VPS_NAME \
      --zone $REGION \
      --machine-type $MACHINE_TYPE \
      --metadata-from-file startup-script=$STARTUP_FILE
  fi
}

function CreateServerVps() {
  local BUILD_NAME=$1
  local REGION=$2

  CreateStartupScript server $BUILD_NAME

  if ($RUN_CLOUD) then
    gcloud compute instances create $SERVER_NAME \
      --tags $SERVER_NETWORK_TAG \
      --zone $REGION \
      --machine-type $MACHINE_TYPE \
      --metadata-from-file startup-script=$STARTUP_FILE

  fi
}

function CreateStartupScript() {
  local TYPE=$1
  local BUILD_NAME=$2
  local SERVER_IP=$3

  cat > $STARTUP_FILE <<-EOM
#! /bin/bash

# setup vps
apt-get update
apt-get install -y screen unzip

# download server
gsutil cp gs://$BUCKET_NAME/$BUILD_NAME.zip $BUILD_NAME.zip
unzip $BUILD_NAME.zip

cd $BUILD_NAME
chmod +x ./$FILE_NAME 

EOM

case $TYPE in

"server" )
cat >> $STARTUP_FILE <<-EOM
screen -d -m -S mirrorServer ./$FILE_NAME -logfile ~/server.log $SERVER_START_ARGS
EOM
  ;;

"client" )
cat >> $STARTUP_FILE <<-EOM
for i in {1..10}; do 
  screen -d -m -S "mirrorClient$i" ./$FILE_NAME -logfile ~/client$i.log $CLIENT_START_ARGS -address $SERVER_IP
done
EOM
  ;;

*)
  echo "CreateStartupScript needs server or client as 1st arg"
esac

}

function GetServerIp() {
  echo "$(gcloud compute instances describe $SERVER_NAME --zone=$SERVER_REGION --format='get(networkInterfaces[0].accessConfigs.natIP)')"
}

####################################################################################
# Config

readonly STARTUP_FILE="./_startup.sh"

# bucket settings
readonly BUCKET_NAME="mirror-builds"
readonly FILE_NAME="mirror_server.x86_64"

# vps settings
readonly MACHINE_TYPE="e2-micro"
readonly SERVER_NETWORK_TAG="mirror-tcp1"

# vps names
readonly SERVER_NAME="mirror-server"
readonly CLIENT_BASE_NAME="mirror-client-"

# temp value so that function runs
EXTERNAL_IP="localhost"

####################################################################################
# Run Settings


readonly BUILD="SyncPosition_server_linux"
readonly SERVER_START_ARGS="-server"
readonly CLIENT_START_ARGS="-client"

readonly SERVER_REGION="europe-west1-b"
# can make 8 vps per zone
# 10 clients per vps
# see https://cloud.google.com/compute/docs/regions-zones
readonly CLIENT_REGIONS=(
  "europe-west1-b"
  "europe-west1-c"
  "europe-west1-d"

  "europe-west2-a"
  "europe-west2-b"
  "europe-west2-c"

  )


####################################################################################
# Run program

readonly RUN_CLOUD=false


# CreateServerVps $BUILD $SERVER_REGION

# EXTERNAL_IP=$(GetServerIp)
CreateManyClientVps $BUILD ${CLIENT_REGIONS[0]} $EXTERNAL_IP 1 7
