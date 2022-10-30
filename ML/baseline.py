import pandas as pd
from tqdm import tqdm
import json

region = 'АЛТАЙСКИЙ КРАЙ'
period = '09.20-11.21'

'''json struct with needed region and period
tnved:
    {
    import_region:  


    }

'''


whole_data = pd.read_csv('whole_data.csv')
unique_tnved = list(whole_data['tnved_6'].unique())
