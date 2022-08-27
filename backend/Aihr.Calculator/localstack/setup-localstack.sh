#!/usr/bin/env bash

set -euo pipefail

# enable debug
# set -x

export AWS_ACCESS_KEY_ID=dummy
export AWS_SECRET_ACCESS_KEY=dummy
AWS_REGION=eu-west-1
AWS_ENDPOINT=http://localhost:4566

echo "Creating courses table..."

aws dynamodb create-table \
    --table-name courses \
    --attribute-definitions AttributeName=id,AttributeType=S \
    --key-schema AttributeName=id,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
    --endpoint ${AWS_ENDPOINT} \
    --region ${AWS_REGION} \
    
echo "Setting up initial data..."

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "db7c87c0-1f6f-4772-95cc-f6f0abdc6df3"},"name": {"S": "Blockchain and HR"},"duration": {"N": "8"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "a5d5ef1d-3996-49c6-b873-20a1a395f501"},"name": {"S": "Compensation & Benefits"},"duration": {"N": "32"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "1b688879-ebb7-49a4-9614-413c4737ca8b"},"name": {"S": "Digital HR"},"duration": {"N": "40"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "e0da5700-065b-44a1-8e6f-2a694439b7cd"},"name": {"S": "Digital HR Strategy"},"duration": {"N": "10"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "5977b554-a723-4756-9d9e-95b46249b5db"},"name": {"S": "Digital HR Transformation"},"duration": {"N": "8"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "c3983e9f-f3ca-4cd0-bd7f-78dab1fd65ab"},"name": {"S": "Diversity & Inclusion"},"duration": {"N": "20"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "c11339f8-661c-43d3-8733-37fc7a9139d8"},"name": {"S": "Employee Experience & Design Thinking"},"duration": {"N": "12"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "17fe402d-a58e-455a-b37c-4d834294d78b"},"name": {"S": "Employer Branding"},"duration": {"N": "6"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "75365d38-8f73-4f73-861b-c6f20efe094e"},"name": {"S": "Global Data Integrity"},"duration": {"N": "12"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "77e36ad5-714f-4db2-89ee-6f0ed155df1f"},"name": {"S": "Hiring & Recruitment Strategy"},"duration": {"N": "15"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "d3f3ce18-085b-4593-8d83-944292e256c3"},"name": {"S": "HR Analytics Leader"},"duration": {"N": "21"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "6178d50f-b9c7-49ed-90ba-50333505241b"},"name": {"S": "HR Business Partner 2.0"},"duration": {"N": "40"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "26112b82-2ca1-4d3c-9869-047515fec3b7"},"name": {"S": "HR Data Analyst"},"duration": {"N": "18"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "92a39750-367f-4612-892b-06ebd852d9f6"},"name": {"S": "HR Data Science in R"},"duration": {"N": "12"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "a92a4b1d-4581-48ed-a616-dca73303ad18"},"name": {"S": "HR Data Visualization"},"duration": {"N": "12"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "e84ffc27-bc7a-4df0-8709-73a3f071aef6"},"name": {"S": "HR Metrics & Reporting"},"duration": {"N": "40"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "8f4f63b8-901b-4a0d-8b16-6c8cc48182e8"},"name": {"S": "Learning & Development"},"duration": {"N": "30"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "f5ef1768-ab3d-4cca-864d-703398ee5666"},"name": {"S": "Organizational Development"},"duration": {"N": "30"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "bec6fd3c-716b-46bb-bc78-c7ad35434b83"},"name": {"S": "People Analytics"},"duration": {"N": "40"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "b7fa4afe-8d7c-428b-9b81-32c23e7ae27b"},"name": {"S": "Statistics in HR"},"duration": {"N": "15"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "ef45fda6-3601-48c2-853d-574295c3a580"},"name": {"S": "Strategic HR Leadership"},"duration": {"N": "34"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "47889c4d-58dd-497b-8491-a0de86d51509"},"name": {"S": "Strategic HR Metrics"},"duration": {"N": "17"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}

aws  dynamodb put-item \
  --table-name courses \
  --item '{"id": {"S": "0dabb4eb-8e2c-46a2-b489-5f9fc6a4fedc"},"name": {"S": "Talent Acquisition"},"duration": {"N": "40"}}' \
  --endpoint ${AWS_ENDPOINT} \
  --region ${AWS_REGION}
  
echo "Creating studies table..."

aws dynamodb create-table \
    --table-name studies \
    --attribute-definitions AttributeName=id,AttributeType=S \
    --key-schema AttributeName=id,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
    --endpoint ${AWS_ENDPOINT} \
    --region ${AWS_REGION} \

echo "Initialization is done!"
