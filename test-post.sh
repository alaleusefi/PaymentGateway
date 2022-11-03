curl --request POST \
--header "Content-Type: application/json" \
--data '{"merchantId":1, "amount":1.2, "Card_Number":"8888-8888-8888-8888", "Card_Cvv":128, "Card_Currency":"GBP", "Card_Expiry_Year":2026, "Card_Expiry_Month":4}' \
localhost:5000/payment && echo
