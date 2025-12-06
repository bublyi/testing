import './style.css'

const firebaseURL = 'https://seethru-e860f-default-rtdb.firebaseio.com/writtenWord.json';

const app = document.querySelector<HTMLDivElement>('#app')!;

app.innerHTML = `
  <div class="container">
    <input type="text" id="wordInput" placeholder="Enter word..." />
    <button id="saveButton" class="checkmark-button">✓</button>
  </div>
`;

const wordInput = document.querySelector<HTMLInputElement>('#wordInput')!;
const saveButton = document.querySelector<HTMLButtonElement>('#saveButton')!;

// Load current word from Firebase
async function loadWord() {
  try {
    const response = await fetch(firebaseURL);
    if (response.ok) {
      const text = await response.text();
      const word = text.trim().replace(/^"|"$/g, ''); // Remove quotes
      wordInput.value = word;
    }
  } catch (error) {
    console.error('Error loading word:', error);
  }
}

// Save word to Firebase
async function saveWord() {
  const word = wordInput.value.trim();
  if (!word) return;

  try {
    const response = await fetch(firebaseURL, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(word),
    });

    if (response.ok) {
      // Visual feedback
      saveButton.classList.add('saved');
      setTimeout(() => {
        saveButton.classList.remove('saved');
      }, 500);
    } else {
      console.error('Error saving word');
    }
  } catch (error) {
    console.error('Error saving word:', error);
  }
}

// Event listeners
saveButton.addEventListener('click', saveWord);
wordInput.addEventListener('keypress', (e) => {
  if (e.key === 'Enter') {
    saveWord();
  }
});

// Load word on startup
loadWord();
