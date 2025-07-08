<template>
  <div>
    <h2>Termékek</h2>
    <ul>
      <li v-for="(item, index) in items" :key="index">
        <template v-if="item.editing">
          <input v-model="item.name" />
          <input v-model="item.price"/>
          <input v-model="item.quantity"/>
          <button @click="saveItem(index)">Mentés</button>
          <button @click="cancelEdit(index)">Mégse</button>
        </template>
        <template v-else>
          <p>Név:</p>{{ item.name }}
          <p>Ár:</p>{{ item.price }}
          <p>Darab:</p>{{ item.quantity }}
          <button @click="editItem(index)">Módosítás</button>
          <button @click="removeItem(index)">Törlés</button>
        </template>
      </li>
    </ul>

    <div v-if="newItem.editing" class="new-item-form">
      <input v-model="newItem.name" placeholder="Új termék neve" />
      <input v-model="newItem.price" placeholder="Ár" />
      <input v-model="newItem.quantity" placeholder="Darabszám" />
      <button @click="addItem">Mentés</button>
      <button @click="cancelNewItem">Mégse</button>
    </div>
    <div v-else>
      <button @click="showNewItemInput">Új elem hozzáadása</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  data() {
    return {
      items: [],
      newItem: {
        name: '',
        price:'',
        quantity:'',
        editing: false
      }
    }
  },
  methods: {
    async fetchItems() {
      const res = await axios.get('https://localhost:5001/api/items')
      this.items = res.data.map(item => ({
        ...item,
        editing: false
      }))
    },
    editItem(index) {
      this.items[index].editing = true
    },
    cancelEdit(index) {
      this.items[index].editing = false
      this.fetchItems()
    },
    async saveItem(index) {
      this.items[index].editing = false
      await this.saveAllItems()
    },
    removeItem(index) {
      this.items.splice(index, 1)
      this.saveAllItems()
    },
    showNewItemInput() {
      this.newItem = { name: '', price:'', quantity:'', editing: true }
    },
    cancelNewItem() {
      this.newItem = { name: '', price:'', quantity:'', editing: false }
    },
    async addItem() {
      if (!this.newItem.name.trim()) return
      this.items.push({ id: 0, name: this.newItem.name, price:parseInt(this.newItem.price), quantity: parseInt(this.newItem.quantity), editing: false })
      this.newItem = { name: '', price:'', quantity:'', editing: false }
      await this.saveAllItems()
    },
    async saveAllItems() {
      try {
        await axios.post('https://localhost:5001/api/items', this.items)
        await this.fetchItems()
      } catch (error) {
        console.error('Hiba mentéskor: ', error)
        alert('Valami nem jó!')
      }
      await axios.post('https://localhost:5001/api/items', this.items)
      await this.fetchItems()
    }
  },
  mounted() {
    this.fetchItems()
  }
}
</script>
