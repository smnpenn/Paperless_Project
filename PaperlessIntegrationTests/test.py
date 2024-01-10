import requests

BASE_URL = 'http://localhost:50352/api/'


def test_five_correspondents():
    response = requests.get(BASE_URL + 'correspondents')

    assert response.status_code == 200

    response_data = response.json()

    assert len(response_data) == 5

    print('test_five_correspondents passed')

def test_create_correspondent():
    new_correspondent = {
        'name': 'created_cor',
        'matching_algorithm': 123,
        'match': 'string',
        'is_insensitive': False,
        'owner': 0
    }
    
    response = requests.post(BASE_URL + 'correspondents', json=new_correspondent)

    assert response.status_code == 200

    assert response.text == '6'

    print('test_create_correspondent passed')

def test_delete_correspondent():
    response = requests.delete(BASE_URL + 'correspondents/3')

    assert response.status_code == 200

    print('test_delete_correspondent passed')

def test_create_tag():
    new_tag = {
        "name": "created tag",
        "color": "created tag color",
        "is_inbox_tag": True,
        "matching_algorithm": 0,
        "match": "created tag match",
        "is_insensitive": True,
        "owner": 0
    }

    response = requests.post(BASE_URL + 'tags', json=new_tag)
    
    assert response.status_code == 200
    
    assert response.text == '1'
    
    print('test_create_tag passed')

def test_tag_amount(amount: int):
    response = requests.get(BASE_URL + 'tags')

    assert response.status_code == 200

    assert len(response.json()) == amount

    print('test_tag_amount (', amount, ') passed')

def test_delete_tag():
    response = requests.delete(BASE_URL + 'tags/1')

    assert response.status_code == 200

    print('test_delete_tag passed')

if __name__ == '__main__':
    test_five_correspondents()
    test_create_correspondent()
    test_delete_correspondent()
    test_five_correspondents()
    test_create_tag()
    test_tag_amount(1)
    test_delete_tag()
    test_tag_amount(0)
