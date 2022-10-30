import pandas as pd
from tqdm import tqdm
import json

def get_dates_from_period(period: str) -> list:
    #example of period '09.20-11.21'
    periods_dates = []
    start_date = period.split('-')[0]
    last_date = period.split('-')[1]
    
region = 'АЛТАЙСКИЙ КРАЙ'
period = '09.20-11.21'


whole_data = pd.read_csv('whole_data.csv')
unique_tnved = list(whole_data['tnved_6'].unique())
stoims = dict()
for i in tqdm(range(len(unique_tnved))):
    stoims[unique_tnved[i]] = sum(whole_data.loc[whole_data['napr'] == 'ИМ' and 
    whole_data['napr'] == unique_tnved[i] and 
    whole_data['period'] in get_dates_from_period(period)]['stoim'])

print(stoims)


