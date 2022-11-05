from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
import time
import pandas as pd
from tqdm import tqdm
df = pd.read_csv('whole_data.csv')
dict_tnveds = {}
unique_tnveds = (df['tnved_6'].unique())
browser = webdriver.Chrome()
browser.get('https://classifikators.ru/tnved')

def get_tnved(code: str) -> str:
    elem = browser.find_element(By.NAME, 'ok_code')
    elem.send_keys(str(code) + Keys.RETURN)
    time.sleep(0.3)
    elem_code = browser.find_element(By.ID, 'frm-search-by-code-msg')
    text = elem_code.text
    elem.clear()
    return(text[17:])


for k in tqdm(range(len(unique_tnveds))):
    tnved = unique_tnveds[k]
    tnved_text = get_tnved(tnved)
    dict_tnveds[tnved] = tnved_text
    print(tnved_text)

df_tnveds = pd.DataFrame([dict_tnveds])

df_tnveds.to_csv('tnveds.csv')

df_tnveds.head()